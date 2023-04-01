

using System.Security.Cryptography.X509Certificates;

namespace Trucks.Common
{
    public static class ModelConstants
    {
        //Truck
        public const int TruckRegNumLength = 8;
        public const string TruckRegNumRegex = @"[A-Z]{2}\d{4}[A-Z]{2}";

        public const int TruckVinNumLength = 17;

        public const int TruckTankCapacityMinValue = 950;
        public const int TruckTankCapacityMaxValue = 1420;

        public const int TruckCargoCapacityMinValue = 5000;
        public const int TruckCargoCapacityMaxValue = 29000;


        //Client
        public const int ClientNameMaxLength = 40; // min 3

        public const int ClientNationalityMaxLength = 40;
        public const int ClientNationalityMinLength = 2;
        //Despatcher
        public const int DespatcherNameMaxLength = 40;
        public const int DespatcherNameMinLength = 2;
    }
}
