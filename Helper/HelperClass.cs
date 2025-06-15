namespace CoffeHub.Helper
{
    public static class HelperClass
    {
        public static bool IsRequiredFieldEmpty { get; set; }
        public static bool IsAccountCreated { get; set; }
        public static bool IsPasswordMismatched { get; set; }
        public static bool IsPhoneNumberInvalid { get; set; }
        public static bool IsPinInvalid { get; set; }
        public static bool IsUserNotExists { get; set; }
        public static bool IsUserAlreadyExists { get; set; }
        public static bool IsAddToCartClicked { get; set; }

        public static bool CanIShowViewCart { get; set; }
        public static int? OverAllQuantity { get; set; }
        public static int TotalPrice { get; set; }

        public static bool IsPaymentCreditCard { get; set; }
        public static bool IsPaymentUpiApp { get; set; }
        public static bool IsPaymentUpiId { get; set; }
        public static bool IsPaymentCod { get; set; }

        public static bool IsPlacedOrderClicked { get; set; }
        public static bool IsAddressSaved { get; set; }
        public static bool IsAdditionalAddressExists { get; set; }
    }
}
