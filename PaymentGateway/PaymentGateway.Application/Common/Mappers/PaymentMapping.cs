using AutoMapper;
using PaymentGateway.Application.Features.Payment.CommandModels;
using PaymentGateway.Application.Features.Payment.QueryModels;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Application.Common.Mappers
{
    /// <summary>
    /// A Class using AutoMapper to map data fields between models
    /// </summary>
    public class PaymentMapping : Profile
    {
        public PaymentMapping()
        {
            CreateMap<CompletedPayment, RetrievePaymentResult>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Money.Currency))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Money.Amount))
                .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => src.Card.CardNumber))
                .ForMember(dest => dest.ExpiryMonth, opt => opt.MapFrom(src => src.Card.ExpiryMonth))
                .ForMember(dest => dest.ExpiryYear, opt => opt.MapFrom(src => src.Card.ExpiryYear));

            CreateMap<ProcessPaymentCommand, Payment>()
                .ForPath(dest => dest.Money.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForPath(dest => dest.Money.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForPath(dest => dest.Card.ExpiryMonth, opt => opt.MapFrom(src => src.ExpiryMonth))
                .ForPath(dest => dest.Card.ExpiryYear, opt => opt.MapFrom(src => src.ExpiryYear))
                .ForPath(dest => dest.Card.CVV, opt => opt.MapFrom(src => src.CVV))
                .ForPath(dest => dest.Card.CardNumber, opt => opt.MapFrom(src => src.CardNumber))
                .ForPath(dest => dest.Card.FullName, opt => opt.MapFrom(src => src.FullName));



        }
    }
}
