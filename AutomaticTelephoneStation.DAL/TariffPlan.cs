namespace AutomaticTelephoneStation.DAL
{
    public static class TariffPlan
    {
        public static string Name { get; } = "Unify";
        public static decimal SubscriptionFee { get; } = 0.66m;
        public static decimal IncomingCalls { get; } = 0.0001m;
        public static decimal OutgoingCalls { get; } = 0.0871m;

    }
}
