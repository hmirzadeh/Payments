using AutoMapper;
using Microsoft.Extensions.Options;
using PaymentGateway.Application.Common.Utility;
using PaymentGateway.Application.Features.Payment.CommandModels;
using PaymentGateway.Application.Features.Payment.QueryModels;
using PaymentGateway.Application.Infrastructure;
using PaymentGateway.Domain.Configurations;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Application.Features.Payment.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IHttpProcessor _httpProcessor;
        private readonly IOptions<AcquiringBank> _config;
        private readonly IMapper _mapper;

        public PaymentService(IHttpProcessor httpProcessor, IOptions<AcquiringBank> config, IMapper mapper)
        {
            _httpProcessor = httpProcessor;
            _config = config;
            _mapper = mapper;
        }
        /// <summary>
        /// A function to map data and submit payment to acquiring bank
        /// </summary>
        /// <param name="paymentCommand">A model containing payment information</param>
        /// <returns>A model representing payment status and Id</returns>
        public async Task<ProcessPaymentResult> ProcessPayment(ProcessPaymentCommand paymentCommand)
        {
            var response = new ProcessPaymentResult();
            try
            {
                var payment = _mapper.Map<Domain.Entities.Payment>(paymentCommand);

                var url = $"{_config.Value.BaseURL}/processPayment";
                var paymentResponse = await _httpProcessor.SendRequest<Domain.Entities.Payment, PaymentResult>(payment, HttpMethod.Post, url);
                response.PaymentId = Guid.NewGuid();
                response.ResultCode = paymentResponse;
                response.Result = paymentResponse.ToString();

            }

            catch (Exception)
            {
                response.ResultCode = PaymentResult.InternalError;
                response.Result = PaymentResult.InternalError.ToString();
            }
            return response;
        }
        /// <summary>
        /// A function to retrieve information of a payment
        /// </summary>
        /// <param name="paymentQuery">An integer representing Id of an already processed payment</param>
        /// <returns>A Model containing a payment information</returns>
        public async Task<RetrievePaymentResult> RetrievePayment(string paymentQuery)
        {
        
            var url = $"{_config.Value.BaseURL}/getPayment/{paymentQuery}";
            var paymentResponse = await _httpProcessor.SendRequest<object, CompletedPayment>("", HttpMethod.Get, url);

            var response = _mapper.Map<RetrievePaymentResult>(paymentResponse);
            response.CardNumber = string.IsNullOrEmpty(response.CardNumber)? string.Empty: MaskingUtility.MaskValues(response.CardNumber, 4);

            return response;
        }
    }
}
