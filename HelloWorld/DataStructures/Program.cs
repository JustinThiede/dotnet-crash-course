// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string[] myGroceryArray = new string[2];

myGroceryArray[0] = "Eggs";
myGroceryArray[1] = "Ice Cream";

Console.WriteLine(myGroceryArray[0]);
Console.WriteLine(myGroceryArray[1]);

string[] mySecondGroceryArray = { "Apple", "Banana" };

Console.WriteLine(mySecondGroceryArray[0]);
Console.WriteLine(mySecondGroceryArray[1]);

List<string> myGroceryList = new List<string>() { "Milk", "Cheese" };

Console.WriteLine(myGroceryList[0]);
Console.WriteLine(myGroceryList[1]);

myGroceryList.Add("Orange");

Console.WriteLine(myGroceryList[2]);

myGroceryList[2] = "test";

Console.WriteLine(value: myGroceryList[2]);

IEnumerable<string> myGroceryIEnumerable = myGroceryList;

Console.WriteLine(value: myGroceryIEnumerable.First()); // IEnumerable doesn't have indexes

string[,] myTwoDimensionalArray = new string[,] {
    { "Apple", "Banana" },
    { "Milk", "Cheese" }
};

Console.WriteLine(myTwoDimensionalArray[0, 0]);

Dictionary<string, string[]> myGroceryDictionary = new Dictionary<string, string[]>() {
    {
        "Dairy", new string[]
        {
            "Cheese", "Ice Creame"
        }
    }
};

Console.WriteLine(myGroceryDictionary["Dairy"][0]);
