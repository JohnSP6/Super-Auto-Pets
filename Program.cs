using System.Runtime.Serialization;
using System.Security.Cryptography;

List<Animals> animals = [];
List<Animals> myTeam = [];
List<Animals> tempStore = [];
List<Animals> enemyTeam = [];
List<Animals> myTempTeam = [];
List<Animals> tempOrder = [];
List<Animals> myOldTeam = [];
int roundNum = 1;
bool firstTime = true;
bool reroll = true;
//Coins are allocated to the player during shop phase.
int coins = 10;

Animals fish = new ("Fish", 3, 2);
Animals ant = new ("Ant", 2, 2);
Animals cricket = new ("Cricket", 3, 1);
Animals beaver = new ("Beaver", 2, 3);
Animals duck = new ("Duck", 3, 2);
Animals mosquito = new ("Mosquito", 2, 2);
Animals pig = new ("Pig", 1, 4);
Animals otter = new ("Otter", 3, 1);
Animals horse = new ("Horse", 1, 2);
Animals empty = new ("   ", 0, 0);
animals.Add(fish);
animals.Add(ant);
animals.Add(cricket);
animals.Add(beaver);
animals.Add(duck);
animals.Add(mosquito);
animals.Add(pig);
animals.Add(otter);
animals.Add(horse);


mainMenu();

void mainMenu(){
    Console.Clear();
    // Displays the Main Menu
    Console.Write("\n        Super Auto Pets \n  ____________________________\n\n  1. Play \n  2. Create Animal \n  3. Exit \n  ____________________________\n\n  ");
    //Prevent invalid input
    try{
        int selection = Convert.ToInt32(Console.ReadLine());
        //Checks if the input given is among the options given
        if (selection < 1 || selection > 3){
        Console.Clear();
        Console.Write("\n        Super Auto Pets \n  ____________________________\n\n\n  Sorry, incorrect selection. Please try again!\n\n  " );
        Console.ReadKey();
        mainMenu();
        } else {
            if (selection == 1){
                //User is sent to the store
                Console.WriteLine(selection);
                gameStore();
            } else if (selection == 2) {
                //User is shown all the animals available
                
            } else if (selection == 3) {
                //Game is closed
                Console.Clear();
                Console.Write("\n        Super Auto Pets \n  ____________________________\n\n      Thanks for Playing! \n  ____________________________\n\n  " );
                Console.ReadKey();
            }
        }
    } catch ( Exception ex) {
        Console.Clear();
        Console.Write("\n  Super Auto Pets \n\n  Sorry, wrong input. Please try again!\n  " +ex.Message);
        myTeam.Clear();
        Console.ReadKey();
        mainMenu();
    }
}

void gameStore(){
    Console.Clear();
    //Refreshes the store until all coins are used.
    while (coins > 0) {
        Console.Clear();
        Console.Write($"\n                      Super Auto Pets \n  __________________________________________________________\n\n  Store: {coins} coins.                                   Round: {roundNum}\n\n" );
        //Randomly selects 3 animals from list to display in the store
        if (firstTime || reroll) {
            tempStore.Clear();
            for (int i = 1; i < 4; i++){
                Random rnd = new Random();
                int randomAnimal  = rnd.Next(0, animals.Count); 
                Console.WriteLine($"  {i}. {animals[randomAnimal].animalName} ");
                //Adds the 3 random animals to a new list so the user can select which to choose based on the UI.
                tempStore.Add(animals[randomAnimal]);
            }
            firstTime = false;
            reroll = false;
            //storePets();
        } else {
            for (int i = 0; i < tempStore.Count; i++){
                Console.WriteLine($"  {i+1}. {tempStore[i].animalName} ");
                //Adds the 3 random animals to a new list so the user can select which to choose based on the UI.
                //tempStore.Add(animals[randomAnimal]);
            }
        }
        //Displays the animals in the player's team.
        Console.Write("\n  __________________________________________________________\n\n  Your Team:  ");
        foreach (var item in myTeam){
            Console.Write($" {item.animalName}  ");
        }
        Console.Write("\n  __________________________________________________________\n ");
        Console.Write("\n\n  Please select which animal to purchase. \n  If you would like to sell a pet, please type 's'. \n  If you would like to re-rell the store, please type 'r'. \n\n  ");
        string? choice1 = Console.ReadLine();
        if (choice1 == "r" || choice1 == "R") {
            coins -= 1;
            reroll = true;
            gameStore();
        } else if (choice1 == "s" || choice1 == "S") {
            sellPets();
            gameStore();
        } else {
            try {
                int choice2 = Convert.ToInt32(choice1);
                if (choice2 > tempStore.Count) {
                    Console.Clear();
                    Console.Write($"\n  Super Auto Pets \n\n  Sorry, invalid option. Please try again! \n  " );
                    //myTeam.Clear();
                    Console.ReadKey();
                    gameStore();
                } else {
                    //Deducts coin value upon selecting an animal.
                    coins -= 3;
                    //The object/animal selected by the player is sent to the player's team list and the temp list is cleared. 
                    myTeam.Add(tempStore[choice2 - 1]);
                    tempStore.RemoveAt(choice2 - 1);
                    //tempStore.Clear();
                }
            } catch (Exception ex) {
                //Console.Clear();
                Console.Write($"\n  Super Auto Pets \n\n  Sorry, wrong input. Please try again!\n " + ex.Message );
                //myTeam.Clear();
                Console.ReadKey();
                gameStore();
            }
        }
    }
    Console.Clear();
    Console.Write("\n                      Super Auto Pets \n  __________________________________________________________\n" );
    Console.Write("\n  Your Team:  ");
    foreach (var item in myTeam){
        Console.Write($" {item.animalName}  ");
    }
    Console.Write("\n  __________________________________________________________\n\n  Would you like to rearrange your team? (Y/N)\n  ");
    string? choice = Console.ReadLine();
    if (choice == "Y" || choice == "y") {
        arrangePets();
        Console.Write("\n  __________________________________________________________\n\n  The battle phase has begun. \n  ");
        Console.ReadKey();
        gameBattle();
    } else {
        gameBattle();
    }
}

void sellPets () {
    Console.Clear();
    Console.Write($"\n                      Super Auto Pets \n  __________________________________________________________\n\n");
    for (int i = 0; i < myTeam.Count; i++){
        Console.WriteLine($"  {i + 1}. {myTeam[i].animalName}  ");
        //Adds the 3 random animals to a new list so the user can select which to choose based on the UI.
    }
    Console.Write("\n\n  Please select which animal to sell. \n  NOTE: Most pets are sold for 1 coin. With the exception of pets with sell abilities. \n\n  ");
    try {
        int choice = Convert.ToInt32(Console.ReadLine());
        if (choice > myTeam.Count) {
            Console.Clear();
            Console.Write($"\n  Super Auto Pets \n\n  Sorry, invalid option. Please try again! \n  " );
            //myTeam.Clear();
            Console.ReadKey();
            sellPets ();
        } else {
            //Deducts coin value upon selecting an animal.
            coins += 1;
            //The object/animal selected by the player is sent to the player's team list and the temp list is cleared. 
            myTeam.RemoveAt(choice - 1);
            //tempStore.Clear();
        }
    } catch (Exception ex) {
        //Console.Clear();
        Console.Write($"\n  Super Auto Pets \n\n  Sorry, wrong input. Please try again!\n " + ex.Message );
        //myTeam.Clear();
        Console.ReadKey();
        sellPets ();
    }
}

void arrangePets() {
    bool repeat = true;
    foreach (var item in myTeam){
        myOldTeam.Add(item);
    }
    while (repeat) {
        Console.Clear();
        Console.Write("\n                      Super Auto Pets \n  __________________________________________________________\n" );
        Console.Write("\n  Your Team:  ");
        foreach (var item in myTeam){
            Console.Write($" {item.animalName}  ");
        }
        Console.Write("\n  __________________________________________________________\n\n");
        for (int i = 0; i < myOldTeam.Count; i++){
            Console.WriteLine($"  {i+1}. {myOldTeam[i].animalName} ");
        }
        Console.Write("\n  __________________________________________________________\n\n  New Order:  ");
        foreach (var item in tempOrder){
            Console.Write($" {item.animalName}  ");
        }
        Console.Write("\n  __________________________________________________________\n\n  Select each pet in the order that you want. \n  ");
        int orderChoice = Convert.ToInt32(Console.ReadLine());
        tempOrder.Add(myOldTeam[orderChoice - 1]);
        myOldTeam.RemoveAt(orderChoice - 1);
        if (myOldTeam.Count == 0) {
            break;
        }
    }
    myTeam.Clear();
    for (int i = 0; i < tempOrder.Count; i++){
        myTeam.Add(tempOrder[i]);
    }
    tempOrder.Clear();
    Console.Clear();
    Console.Write("\n                      Super Auto Pets \n  __________________________________________________________\n" );
    Console.Write("\n  Your Team:  ");
    foreach (var item in myTeam){
        Console.Write($" {item.animalName}  ");
    }  
}

void gameBattle(){
    foreach (var item in myTeam){
        var animal = item;
        myTempTeam.Add(new Animals(animal.animalName, animal.animalHealth, animal.animalDmg));
    }
    for (int i = 1; i < 4; i++){
        Random rnd = new Random();
        int randomAnimal  = rnd.Next(0, animals.Count);
        var animal = animals[randomAnimal]; 
        enemyTeam.Add(new Animals(animal.animalName, animal.animalHealth, animal.animalDmg));
    }
    while (myTempTeam.Count > 0 || enemyTeam.Count > 0) {
        if (myTempTeam.Count == 0){
            Console.Write("\n  You have lost!");
            roundNum++;
            Console.ReadKey();
            gameStore();
        } else if (enemyTeam.Count == 0){
            Console.Write("\n  You have Won!");
            Console.ReadKey();
            roundNum++;
            gameStore();
        } else {
            Console.Clear();
            Console.Write($"\n                      Super Auto Pets \n  __________________________________________________________\n\n                      Battle Phase! \n" );
            Console.Write("\n  Player \n  ------\n [ ");
            foreach (var item in myTempTeam){
                Console.Write($"  {item.animalName}  ");
            }
            Console.Write(" ]");
            Console.Write($"\n\n  Name  : {myTempTeam.First().animalName}\n  Health: {myTempTeam.First().animalHealth}\n  Damage: {myTempTeam.First().animalDmg}");
            Console.Write("\n  __________________________________________________________\n\n  Enemy \n  ------\n  [");
            foreach (var item in enemyTeam){
                Console.Write($"  {item.animalName}  ");
            }
            Console.Write(" ]");
            Console.Write($"\n\n  Name  : {enemyTeam.First().animalName}\n  Health: {enemyTeam.First().animalHealth}\n  Damage: {enemyTeam.First().animalDmg}");
            Console.Write("\n  __________________________________________________________\n\n\n  Battle Log \n  ----------\n");
            Console.Write($"\n  Enemy {enemyTeam.First().animalName} > {myTempTeam.First().animalName}  {enemyTeam.First().animalDmg} damage.\n  ");
            //Console.Write($"\n  The enemy's {enemyTeam.First().animalName} hurt your {myTempTeam.First().animalName} for {enemyTeam.First().animalDmg} damage.\n ");
            myTempTeam.First().animalHealth = myTempTeam.First().animalHealth - enemyTeam.First().animalDmg;
            //Console.ReadKey();
            Console.Write($"\n  Ally {myTempTeam.First().animalName} > {enemyTeam.First().animalName}  {myTempTeam.First().animalDmg} damage.\n  ");
            //Console.Write($"\n  Your {myTempTeam.First().animalName} hurt the enemy's {enemyTeam.First().animalName} for {myTempTeam.First().animalDmg} damage.\n ");
            enemyTeam.First().animalHealth = enemyTeam.First().animalHealth - myTempTeam.First().animalDmg;
            if (myTempTeam.First().animalHealth <= 0 && enemyTeam.First().animalHealth <= 0) {
                Console.Write($"\n  Both your {myTempTeam.First().animalName} and enemy's {enemyTeam.First().animalName} has died.\n  ");
                myTempTeam.Remove(myTempTeam.First());
                enemyTeam.Remove(enemyTeam.First());
                Console.ReadKey();
            } else if (enemyTeam.First().animalHealth <= 0) {
                Console.Write($"\n  The enemy's {enemyTeam.First().animalName} has died.\n  ");
                enemyTeam.Remove(enemyTeam.First());
                Console.ReadKey();
            } else if (myTempTeam.First().animalHealth <= 0) {
                Console.Write($"\n  Your {myTempTeam.First().animalName} has died.\n  ");
                myTempTeam.Remove(myTempTeam.First());
                Console.ReadKey();
            }
            else {
                Console.ReadKey();
            }
        }
    }
    Console.Write("\n  The round concluded in a draw.");
    roundNum++;
    Console.ReadKey();
    gameStore(); 
}

public class Animals
{
    public string animalName { get; set; }
    public int animalHealth { get; set; }
    public int animalDmg { get; set; }

    public Animals(string animName, int animHealth, int animDmg)
    {
        this.animalName = animName;
        this.animalHealth = animHealth;
        this.animalDmg = animDmg;
    }
}

// Functionality: 
// - Make a Main Menu 
//   - Start the game [Done]
//   - Add an animal database where user can view all the animals available
//   - Exit the game [Done]
// - Make a shop
//   - Add 3 - 5 random animals from the full animal list to the store. [Done -3 animals]
//   - Add coin system [Done]
//   - Add player's team with rearrangement feature
//   - Add re-roll store feature [Done]
//   - Add sell option [Done]
// - Make a battle phase
//   - Generate random enemy team [Done]
