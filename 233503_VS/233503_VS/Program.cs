using System;
using System.Collections.Generic;

namespace Mids
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public User(int id, string name, string phone)
        {
            Id = id;
            Name = name;
            Phone = phone;
        }

        public abstract void Register();
        public abstract void Login();
        public abstract void DisplayProfile();
    }

    public class Rider : User
    {
        public List<string> RideHistory { get; set; } = new List<string>();

        public Rider(int id, string name, string phone) : base(id, name, phone) { }

        public override void Register()
        {
            Console.WriteLine($"{Name} is successfully registered as a Rider.");
        }

        public override void Login()
        {
            Console.WriteLine($"{Name} is logged in as a Rider.");
        }

        public override void DisplayProfile()
        {
            Console.WriteLine($"Rider Profile: Id={Id}, Name={Name}, Phone={Phone}, RideHistory={string.Join(", ", RideHistory)}");
        }

        public void RequestRide(string destination)
        {
            Console.WriteLine($"{Name} requested a ride to {destination}.");
            RideHistory.Add(destination);
        }

        public void ViewRideHistory()
        {
            Console.WriteLine($"Ride History for {Name}: {string.Join(", ", RideHistory)}");
        }
    }

    public class Driver : User
    {
        public int DriverId { get; set; }
        public string VehicleDetails { get; set; }
        public bool IsAvailable { get; set; } = true;
        public List<string> TripHistory { get; set; } = new List<string>();

        public Driver(int id, string name, string phone, int driverId, string vehicleDetails) : base(id, name, phone)
        {
            DriverId = driverId;
            VehicleDetails = vehicleDetails;
        }

        public override void Register()
        {
            Console.WriteLine($"{Name} is successfully registered as a Driver.");
        }

        public override void Login()
        {
            Console.WriteLine($"{Name} logged in as a Driver.");
        }

        public override void DisplayProfile()
        {
            Console.WriteLine($"Driver Profile: Id={Id}, Name={Name}, Phone={Phone}, Vehicle={VehicleDetails}, Available={IsAvailable}, TripHistory={string.Join(", ", TripHistory)}");
        }

        public void AcceptRide(string riderName)
        {
            if (IsAvailable)
            {
                Console.WriteLine($"{Name} accepted a ride request from {riderName}.");
                TripHistory.Add(riderName);
                IsAvailable = false; 
            }
            else
            {
                Console.WriteLine($"{Name} is not available for rides.");
            }
        }

        public void ToggleAvailability()
        {
            IsAvailable = !IsAvailable;
            Console.WriteLine($"{Name} is now {(IsAvailable ? "available" : "not available")} for rides.");
        }

        public void CompleteTrip()
        {
            Console.WriteLine($"{Name} completed a trip.");
        }

        public void ViewTripHistory()
        {
            Console.WriteLine($"Trip History for {Name}: {string.Join(", ", TripHistory)}");
        }
    }

    public class Trip
    {
        public int TripId { get; set; }
        public string RiderName { get; set; }
        public string DriverName { get; set; }
        public string StartLocation { get; set; }
        public string Destination { get; set; }
        public int Fare { get; set; }
        public bool Status { get; set; }

        public Trip(int tripId, string riderName, string driverName, string startLocation, string destination, int fare)
        {
            TripId = tripId;
            RiderName = riderName;
            DriverName = driverName;
            StartLocation = startLocation;
            Destination = destination;
            Fare = fare;
            Status = false;
        }

        public void StartTrip()
        {
            Status = true;
            Console.WriteLine($"Trip started from {StartLocation} to {Destination}.");
        }

        public void EndTrip()
        {
            Status = false;
            Console.WriteLine($"Trip ended. Fare is: {Fare}");
        }

        public void DisplayTripDetails()
        {
            Console.WriteLine($"Trip Details: Id={TripId}, Rider={RiderName}, Driver={DriverName}, From={StartLocation}, To={Destination}, Fare={Fare}, Status={(Status ? "Ongoing" : "Completed")}");
        }
    }

    public class RideSharingSystem
    {
        public List<Rider> RegisteredRiders { get; set; } = new List<Rider>();
        public List<Driver> RegisteredDrivers { get; set; } = new List<Driver>();
        public List<Trip> AvailableTrips { get; set; } = new List<Trip>();

        public void RegisterUser(User user)
        {
            if (user is Rider)
            {
                RegisteredRiders.Add((Rider)user);
            }
            else if (user is Driver)
            {
                RegisteredDrivers.Add((Driver)user);
            }
        }

        public void RequestRide(Rider rider, string destination)
        {
            foreach (var driver in RegisteredDrivers)
            {
                if (driver.IsAvailable)
                {
                    var fare = CalculateFare(rider, destination);
                    var trip = new Trip(AvailableTrips.Count + 1, rider.Name, driver.Name, "Start Location", destination, fare);
                    AvailableTrips.Add(trip);
                    driver.AcceptRide(rider.Name);
                    trip.StartTrip();
                    Console.WriteLine($"{rider.Name} requested a ride to {destination}. Trip ID: {trip.TripId}");
                    return; 
                }
            }
            Console.WriteLine("No drivers available at the moment.");
        }

        public void CompleteTrip(int tripId)
        {
            var trip = AvailableTrips.Find(t => t.TripId == tripId);
            if (trip != null)
            {
                trip.EndTrip();
                var driver = RegisteredDrivers.Find(d => d.Name == trip.DriverName);
                driver.IsAvailable = true; 
                AvailableTrips.Remove(trip); 
            }
            else
            {
                Console.WriteLine("Trip not found.");
            }
        }

        public void DisplayAllTrips()
        {
            foreach (var trip in AvailableTrips)
            {
                trip.DisplayTripDetails();
            }
        }

        private int CalculateFare(Rider rider, string destination)
        {
           
            return 100; 
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var rideSharingSystem = new RideSharingSystem();

            while (true)
            {
                Console.WriteLine("Ride Sharing System Menu:");
                Console.WriteLine("1. Register as Rider");
                Console.WriteLine("2. Register as Driver");
                Console.WriteLine("3. Accept a Ride (for Drivers)");
                Console.WriteLine("4. Complete a Trip");
                Console.WriteLine("5. View Ride History (for Riders)");
                Console.WriteLine("6. View Trip History (for Drivers)");
                Console.WriteLine("7. Request a Ride");
                Console.WriteLine("8. Display All Trips");
                Console.WriteLine("9. Exit");

                Console.Write("Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter Rider Id: ");
                        int riderId = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter Rider Name: ");
                        string riderName = Console.ReadLine();
                        Console.Write("Enter Rider Phone: ");
                        string riderPhone = Console.ReadLine();

                        var rider = new Rider(riderId, riderName, riderPhone);
                        rideSharingSystem.RegisterUser(rider);
                        break;

                    case 2:
                        Console.Write("Enter Driver Id: ");
                        int driverId = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter Driver Name: ");
                        string driverName = Console.ReadLine();
                        Console.Write("Enter Driver Phone: ");
                        string driverPhone = Console.ReadLine();
                        Console.Write("Enter Driver Vehicle Details: ");
                        string vehicleDetails = Console.ReadLine();

                        var driver = new Driver(driverId, driverName, driverPhone, driverId, vehicleDetails);
                        rideSharingSystem.RegisterUser(driver);
                        break;

                    case 3:
                        Console.Write("Enter Driver Id: ");
                        int driverRequestId = Convert.ToInt32(Console.ReadLine());

                        var driverRequest = rideSharingSystem.RegisteredDrivers.Find(d => d.Id == driverRequestId);
                        if (driverRequest != null)
                        {
                            driverRequest.ToggleAvailability();
                        }
                        else
                        {
                            Console.WriteLine("Driver not found.");
                        }
                        break;

                    case 4:
                        Console.Write("Enter Trip Id to complete: ");
                        int completeTripId = Convert.ToInt32(Console.ReadLine());
                        rideSharingSystem.CompleteTrip(completeTripId);
                        break;

                    case 5:
                        Console.Write("Enter Rider Id: ");
                        int viewRideHistoryId = Convert.ToInt32(Console.ReadLine());

                        var viewRideHistoryRider = rideSharingSystem.RegisteredRiders.Find(r => r.Id == viewRideHistoryId);
                        if (viewRideHistoryRider != null)
                        {
                            viewRideHistoryRider.ViewRideHistory();
                        }
                        else
                        {
                            Console.WriteLine("Rider not found.");
                        }
                        break;

                    case 6:
                        Console.Write("Enter Driver Id: ");
                        int viewTripHistoryId = Convert.ToInt32(Console.ReadLine());

                        var viewTripHistoryDriver = rideSharingSystem.RegisteredDrivers.Find(d => d.Id == viewTripHistoryId);
                        if (viewTripHistoryDriver != null)
                        {
                            viewTripHistoryDriver.ViewTripHistory();
                        }
                        else
                        {
                            Console.WriteLine("Driver not found.");
                        }
                        break;

                    case 7:
                        Console.Write("Enter Rider Id: ");
                        int riderRequestId = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter Destination: ");
                        string destination = Console.ReadLine();

                        var riderRequest = rideSharingSystem.RegisteredRiders.Find(r => r.Id == riderRequestId);
                        if (riderRequest != null)
                        {
                            rideSharingSystem.RequestRide(riderRequest, destination);
                        }
                        else
                        {
                            Console.WriteLine("Rider not found.");
                        }
                        break;

                    case 8:
                        rideSharingSystem.DisplayAllTrips();
                        break;

                    case 9:
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}