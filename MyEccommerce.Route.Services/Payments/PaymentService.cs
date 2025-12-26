using AutoMapper;
using Microsoft.Extensions.Options;
using MyEccommerce.Route.Domain.Contracts;
using MyEccommerce.Route.Domain.Entities.Orders;
using MyEccommerce.Route.Domain.Exceptions.BasketExceptions;
using MyEccommerce.Route.Domain.Exceptions.Orders;
using MyEccommerce.Route.Domain.Exceptions.ProductExceptions;
using MyEccommerce.Route.Services.Abstractions.Notifications;
using MyEccommerce.Route.Services.Abstractions.Payments;
using MyEccommerce.Route.Services.Specification;
using MyEccommerce.Route.Shared.Dtos.BasketDtos;
using MyEccommerce.Route.Shared.StripeData;
using Stripe;
using Stripe.Climate;
using Order = MyEccommerce.Route.Domain.Entities.Orders.Order;
using Product = MyEcommerce.Route.Domain.Entities.Products.Product;

namespace MyEccommerce.Route.Services.Payments
{
    public class PaymentService(IBasketRepository _basketRepository, IUnitOfWok _unitOfWok, IMapper _mapper, IOptions<StripeOption> _options,INotificationService _notificationService) : IPaymentService
    {
        public async Task<BasketDto> CreatePaymentIntentAsync(string basketId)
        {
           var basket=await _basketRepository.GetBasketAsync(basketId);
            if (basket is null) throw new BasketNotFoundException(basketId);

            if (!basket.DeliveryMethodId.HasValue) throw new DeleteBasketBadRequestException();
            var deliveryMethod = await _unitOfWok.GetRepository<int, DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
            if (deliveryMethod is null) throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value) ;

            var ProductRepo = _unitOfWok.GetRepository<int,Product>();

            foreach (var item in basket.Items)
            {
                var product = await ProductRepo.GetByIdAsync(item.Id);
                if (product is null) throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
            }
            var subTotal = basket.Items.Sum(i => i.Price * i.Quantity);
            var amount = subTotal + deliveryMethod.Cost;

            var stripeOptions = _options.Value;

            StripeConfiguration.ApiKey = stripeOptions.SecretKey;

            var paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if(basket.PaymentIntentId is null)
            {
                var paymentIntentCreateOptions = new PaymentIntentCreateOptions()
                {
                    Amount = (long)amount * 100,
                    PaymentMethodTypes=new List<string>() { "card"},
                    Currency="usd"
                };
              paymentIntent=  await  paymentIntentService.CreateAsync(paymentIntentCreateOptions);
            }

            else
            {
                var paymentIntentUpdateOptions = new PaymentIntentUpdateOptions()
                { Amount = (long)amount * 100 };
              paymentIntent=await  paymentIntentService.UpdateAsync(basket.PaymentIntentId,paymentIntentUpdateOptions);
            }
            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;
            basket.ShippingCost = deliveryMethod.Cost;
             basket=  await _basketRepository.UpdateBasketAsync(basket);
            return _mapper.Map<BasketDto>(basket);
        
        }

        public async Task UpdateOrderPaymentStatusAsync(string json, string header)
        {
            string endpointSecret = _options.Value.EndPointSecret;

            Stripe.Event stripeEvent;

            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    header,
                    endpointSecret
                );
            }
            catch (Exception)
            {
                throw;
            }

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            if (paymentIntent == null)
            {
                throw new Exception("PaymentIntent is null in webhook");
            }

            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentSucceeded:
                    await UpdatePaymentIntentSucceededAsync(paymentIntent.Id);
                    break;

                case EventTypes.PaymentIntentPaymentFailed:
                    await UpdatePaymentIntentFailedAsync(paymentIntent.Id);
                    break;

                default:
                    Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
                    break;
            }
        }
        private async Task UpdatePaymentIntentSucceededAsync(string? paymentIntentId)
        {
            var spec = new OrderWithPaymentIntentSpecification(paymentIntentId);
          var OrderRepo=  _unitOfWok.GetRepository<Guid,Order>();
            var order=  await  OrderRepo.GetByIdAsync(spec)??throw new OrderNotFoundPaymentIntent();

            order.Status=OrderStatus.PaymentSuccess;

            
            OrderRepo.Update(order);
            await _unitOfWok.SaveChangeAsync();
          await  _notificationService.SendNotificationAsync(order.userEmail, "Payment Successful",
         $"Payment for order #{order.Id} was successful ");

        }
   
       private async Task UpdatePaymentIntentFailedAsync(string? paymentIntentId)
        {

            var OrderRepo = _unitOfWok.GetRepository<Guid, Order>();

            var spec = new OrderWithPaymentIntentSpecification(paymentIntentId);
            var order=await OrderRepo.GetByIdAsync(spec)??throw new OrderNotFoundPaymentIntent();

            order.Status = OrderStatus.PaymentFailed;
            OrderRepo.Update(order);
            await  _unitOfWok.SaveChangeAsync();
            await _notificationService.SendNotificationAsync(order.userEmail, "Payment Failed",
         $"Payment for order #{order.Id} was failed ");
        }



    }
}
