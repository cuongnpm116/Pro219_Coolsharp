﻿namespace CustomerWebApp.Components.Payment.Dtos;

public class PaymentResponse
{
    public bool Success { get; set; }
    public string PaymentMethod { get; set; } = "";
    public string OrderDescription { get; set; } = "";
    public string OrderId { get; set; } = "";
    public string PaymentId { get; set; } = "";
    public string TransactionStatus { get; set; } = "";
    public string TransactionId { get; set; } = "";
    public string Token { get; set; } = "";
    public string VnPayResponseCode { get; set; } = "";
    public decimal Amount { get; set; }
    public DateTime PayDate { get; set; }
    public string BankCocde { get; set; } = "";
}
