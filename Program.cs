using System.Runtime.Serialization;
using System.Security.Cryptography;

//Creates all lists
List<Animals> animals = [];
List<Animals> myTeam = [];
List<Animals> tempStore = [];
List<Animals> enemyTeam = [];
List<Animals> myTempTeam = [];
List<Animals> tempOrder = [];
List<Animals> myOldTeam = [];
List<Animals> frozenPets = [];

//Keeps tracks of the number of rounds
int roundNum = 1;
//Checks if this is the first time the player entered the store
bool firstTime = true;
//Checks and see if the player rerolled the store
bool reroll = true;
//Coins are allocated to the player during shop phase.
int coins = 10;
//Set the number of wins to 0
int wins = 0;
//Player has a health of 5
int health = 5;

//Populates the list with animal data
List<string> lines = File.ReadAllLines("Animals.txt").ToList();
foreach (string line in lines) {
    string[] items = line.Split(',');
    animals.Add(new Animals(items[0], Convert.ToInt32(items[1]), Convert.ToInt32(items[2])));
}
mainMenu();

#region Main Menu 

void mainMenu(){
    firstTime = true;
    frozenPets.Clear();
    Console.Clear();
    // Displays the Main Menu
    Console.Write("\n        Super Auto Pets \n  ____________________________\n\n  1. Play \n  2. Animal Encyclopedia \n  3. Exit \n  ____________________________\n\n  ");
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
                gameStore();
            } else if (selection == 2) {
                //User is shown all the animals available
                animalEncyclopedia();
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
        Console.ReadKey();
        mainMenu();
    }
}

void animalEncyclopedia(){
    Console.Clear();
    Console.Write("\n        Super Auto Pets \n  ____________________________\n\n\n      Animal Encyclopedia\n\n" );
    foreach (var item in animals){
        //Console.Write($"  {item.animalName}  ");
        Console.Write($"\n  Name  : {item.animalName}\n  Health: {item.animalHealth}\n  Damage: {item.animalDmg}\n  __________________________________");
    }
    Console.ReadKey();
    mainMenu();
}

#endregion

#region Game Store
//Opens the game store. 
void gameStore(){
    Console.Clear();
    //Recalls the store until all coins are used.
    while (coins > 0) {
        Console.Clear();
        Console.Write($"\n                      Super Auto Pets \n  __________________________________________________________\n\n  Store: {coins} coins.                                   Round: {roundNum}\n\n" );
        //Checks and see if this is the first time the user enters the store or rerolls. 
        if (firstTime || reroll) {
            //Clears the list that saved the previous store's data
            tempStore.Clear();
            if (roundNum > 2) {
                //Randomly selects 5 animals from full animal list to display in the store
                for (int i = 1; i < 6; i++){
                    Random rnd = new Random();
                    int randomAnimal  = rnd.Next(0, animals.Count); 
                    Console.WriteLine($"  {i}. {animals[randomAnimal].animalName} ");
                    //Adds the 5 random animals to a new list so the user can select which to choose based on the UI.
                    tempStore.Add(animals[randomAnimal]);
                }
            } else if (roundNum > 1) {
                //Randomly selects 4 animals from full animal list to display in the store
                for (int i = 1; i < 5; i++){
                    Random rnd = new Random();
                    int randomAnimal  = rnd.Next(0, animals.Count); 
                    Console.WriteLine($"  {i}. {animals[randomAnimal].animalName} ");
                    //Adds the 4 random animals to a new list so the user can select which to choose based on the UI.
                    tempStore.Add(animals[randomAnimal]);
                }
            } else {

                if (frozenPets.Count != 0) { 
                    foreach (var item in frozenPets){
                        Console.Write($"  {frozenPets.Count}. {item.animalName} [FROZEN] \n");
                    }
                }
                //Randomly selects 3 animals from full animal list to display in the store
                for (int i = 1; i < (4 - frozenPets.Count); i++)
                {
                    Random rnd = new Random();
                    int randomAnimal = rnd.Next(0, animals.Count);
                    Console.WriteLine($"  {i + frozenPets.Count}. {animals[randomAnimal].animalName} ");
                    //Adds the 3 random animals to a new list so the user can select which to choose based on the UI.
                    tempStore.Add(animals[randomAnimal]);
                }
            }
            //Changes the bool values to false to make it so that when the store is refreshed, the store animals are saved
            firstTime = false;
            reroll = false;
        } else {
            foreach (var item in frozenPets){
                Console.Write($"  {frozenPets.Count}. {item.animalName} [FROZEN] \n");
            }
            //Displays the store animals
            for (int i = 0; i < tempStore.Count; i++){
                Console.WriteLine($"  {i + frozenPets.Count +1}. {tempStore[i].animalName} ");
            }
        }
        //Displays the animals in the player's team.
        Console.Write("\n  __________________________________________________________\n\n  Your Team:  ");
        foreach (var item in myTeam){
            Console.Write($" {item.animalName}  ");
        }
        Console.Write($"\n  __________________________________________________________\n\n  Wins: {wins}                                          Health: {health} \n\n  Please select which animal to purchase. \n  Type 's' to sell a pet. \n  Type 'r' to re-roll store. \n  Type 'f' to freeze pets.  \n  Type 'b' to start the battle.  \n\n  ");
        string? choice1 = Console.ReadLine();
        //If the user chooses to reroll the shop
        if (choice1 == "r" || choice1 == "R")
        {
            coins -= 1;
            reroll = true;
            gameStore();
            //If the user chooses to sell a pet
        }
        else if (choice1 == "s" || choice1 == "S")
        {
            sellPets();
            gameStore();
        }
        else if (choice1 == "b" || choice1 == "B")
        {
            startBattle();
        }
        else if (choice1 == "f" || choice1 == "F")
        {
            Console.WriteLine("  Which pet would you like to freeze?\n  ");
            string? choice3 = Console.ReadLine();
            int choice = Convert.ToInt32(choice3);
            frozenPets.Add(tempStore[choice - 1]);
            tempStore.Remove(tempStore[choice - 1]);
        }
        else
        {
            try
            {
                
                int choice2 = Convert.ToInt32(choice1); 

                //Checks if the user's option exceeds the options given
                if (choice2 > (tempStore.Count + frozenPets.Count) ) //CHANGED THIS [FROZE#]
                {
                    Console.Clear();
                    Console.Write($"\n  Super Auto Pets \n\n  Sorry, invalid option. Please try again! \n  ");
                    Console.ReadKey();
                    gameStore();
                }
                else
                {
                    if (myTeam.Count == 5)
                    {
                        //Console.Clear();
                        Console.Write($"  __________________________________________________________\n\n               Your team is already full. \n   __________________________________________________________");
                        Console.ReadKey();
                        gameStore();
                    }
                    else
                    {
                        //Deducts coin value upon selecting an animal.
                        coins -= 3;
                        //The object/animal selected by the player is sent to the player's team list and the temp list is cleared. 

                        if (frozenPets.Count == 0)
                        {
                            myTeam.Add(tempStore[choice2 - 1]);
                            tempStore.RemoveAt(choice2 - 1);
                        }
                        else
                        {
                            if (choice2 <= frozenPets.Count)
                            {
                                myTeam.Add(frozenPets[choice2 - 1]);
                                frozenPets.RemoveAt(choice2 - 1);
                            }
                            else
                            { 
                                myTeam.Add(tempStore[choice2 - 1 - frozenPets.Count]);
                                tempStore.RemoveAt(choice2 - 1 - frozenPets.Count);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.Write($"\n  Super Auto Pets \n\n  Sorry, wrong input. Please try again! \n\n  {ex.Message}");
                Console.ReadKey();
                gameStore();
            }
        }
    }
    startBattle();
}

//Selling pets function. 
void sellPets () {
    Console.Clear();
    Console.Write($"\n                      Super Auto Pets \n  __________________________________________________________\n\n");
    //Checks if there are any pets in the player's team
    if (myTeam.Count == 0)
    {
        Console.Write("   You have no pets to sell. \n  ");
        Console.Write("\n  __________________________________________________________\n\n  Press any key to return to the store. \n  ");
        Console.ReadKey();
        gameStore();
        return;
    }
    else
    {
        //Displays all the pets in the players team
        for (int i = 0; i < myTeam.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {myTeam[i].animalName}  ");
        }
        Console.Write("\n  __________________________________________________________\n\n ");
        Console.Write("\n\n  Please select which animal to sell. \n  NOTE: Most pets are sold for 1 coin. With the exception of pets with sell abilities. \n ");
        Console.Write("\n  Press 'B' to go back to the store. \n\n  ");

        string? userInput = Console.ReadLine();
        if (userInput.ToLower().Trim() == "b")
        {
            gameStore();
            return;
        }
        else
        {
            try
            {
                int choice = Convert.ToInt32(userInput);
                if (choice > myTeam.Count)
                {
                    Console.Clear();
                    Console.Write($"\n  Super Auto Pets \n\n  Sorry, invalid option. Please try again! \n  ");
                    Console.ReadKey();
                    sellPets();
                }
                else
                {
                    //Adds coin value upon selecting an animal to sell.
                    coins += 1;
                    //The animal selected is removed from the player's team. 
                    Console.Write($"\n  Your {myTeam[choice-1].animalName} has been sold. \n  ");
                    Console.ReadKey();
                    myTeam.RemoveAt(choice - 1);
                    sellPets();
                }
            }
            catch (Exception ex)
            {
                Console.Write($"\n\n  Sorry, wrong input. Please try again!  \n\n  {ex.Message} ");
                Console.ReadKey();
                sellPets();
            }
        }
        
    }

}

//Arranging pets order function. 
void arrangePets() {
    bool repeat = true;
    foreach (var item in myTeam){
        myOldTeam.Add(item);
    }
    while (repeat) {
        Console.Clear();
        Console.Write("\n                      Super Auto Pets \n  __________________________________________________________\n\n  Your Team:  " );
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
        try {
            int orderChoice = Convert.ToInt32(Console.ReadLine());
            tempOrder.Add(myOldTeam[orderChoice - 1]);
            myOldTeam.RemoveAt(orderChoice - 1);
            if (myOldTeam.Count == 0) {
                myTeam.Clear();
                for (int i = 0; i < tempOrder.Count; i++){
                    myTeam.Add(tempOrder[i]);
                }
                tempOrder.Clear();
                Console.Clear();
                Console.Write("\n                      Super Auto Pets \n  __________________________________________________________\n\n  Your Team:  " );
                foreach (var item in myTeam){
                    Console.Write($" {item.animalName}  ");
                }
                repeat = false;
            }
        } catch (Exception ex) {
            Console.Clear();
            Console.Write($"\n  Super Auto Pets \n\n  Sorry, wrong input. Please try again! \n\n  {ex.Message}" );
            myOldTeam.Clear();
            tempOrder.Clear();
            repeat = false;
            Console.ReadKey();
            arrangePets();
        }
    }
}

//Starts the battle. 
void startBattle () {
    Console.Clear();
    frozenPets.Clear();
    //Displays player's team in current order
    Console.Write("\n                      Super Auto Pets \n  __________________________________________________________\n\n  Your Team:  " );
    foreach (var item in myTeam){
        Console.Write($" {item.animalName}  ");
    }
    //Prompts the player to see if they want to rearrange their team.
    Console.Write("\n  __________________________________________________________\n\n  Would you like to rearrange your team? (Y/N)\n  ");
    string? choice = Console.ReadLine();
    if (choice == "Y" || choice == "y") {
        arrangePets();
        //Goes to the battle phase once user arranged their team. 
        Console.Write("\n  __________________________________________________________\n\n  The battle phase has begun. \n  ");
        Console.ReadKey();
        gameBattle();
    } else {
        //Immediately goes to the battle stage
        gameBattle();
    }
}
#endregion

#region Game Battle
void gameBattle(){
    foreach (var item in myTeam){
        myTempTeam.Add(new Animals(item.animalName, item.animalHealth, item.animalDmg));
    }
    if (roundNum == 1) {
        for (int i = 0; i < 3; i++){
            Random rnd = new Random();
            int randomAnimal  = rnd.Next(0, animals.Count);
            enemyTeam.Add(new Animals(animals[randomAnimal].animalName, animals[randomAnimal].animalHealth, animals[randomAnimal].animalDmg));
        }
    } else {
       for (int i = 0; i < 5; i++){
            Random rnd = new Random();
            int randomAnimal  = rnd.Next(0, animals.Count);
            enemyTeam.Add(new Animals(animals[randomAnimal].animalName, animals[randomAnimal].animalHealth, animals[randomAnimal].animalDmg));
        } 
    }
    
    while (myTempTeam.Count > 0 || enemyTeam.Count > 0) {
        //TO DO - Further testing should be done on draws
        if (myTempTeam.Count == 0){
            Console.Write("\n  You have lost!");
            health -= 1;
            Console.ReadKey();
            gameOver();
        } else if (enemyTeam.Count == 0){
            Console.Write("\n  You have Won!");
            Console.ReadKey();
            wins += 1;
            myTempTeam.Clear();
            gameOver();
        } else {
            Console.Clear();
            Console.Write($"\n                      Super Auto Pets \n  __________________________________________________________\n\n                      Battle Phase! \n\n  Player \n  ------\n [ " );
            foreach (var item in myTempTeam){
                Console.Write($"  {item.animalName}  ");
            }
            Console.Write($" ]\n\n  Name  : {myTempTeam.First().animalName}\n  Health: {myTempTeam.First().animalHealth}\n  Damage: {myTempTeam.First().animalDmg}\n  __________________________________________________________\n\n  Enemy \n  ------\n  [");
            foreach (var item in enemyTeam){
                Console.Write($"  {item.animalName}  ");
            }
            Console.Write($" ]\n\n  Name  : {enemyTeam.First().animalName}\n  Health: {enemyTeam.First().animalHealth}\n  Damage: {enemyTeam.First().animalDmg}\n  __________________________________________________________\n\n\n  Battle Log \n  ----------\n\n  Enemy {enemyTeam.First().animalName} > {myTempTeam.First().animalName}  {enemyTeam.First().animalDmg} damage.\n\n  Ally {myTempTeam.First().animalName} > {enemyTeam.First().animalName}  {myTempTeam.First().animalDmg} damage.\n  ");
            myTempTeam.First().animalHealth = myTempTeam.First().animalHealth - enemyTeam.First().animalDmg;
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
    firstTime = true;
    coins = 10;
    Console.ReadKey();
    gameStore(); 
}
#endregion

#region End Game
void gameOver(){
    Console.Clear();
    Console.Write("\n                      Super Auto Pets \n  __________________________________________________________\n\n  Your Team:  " );
    foreach (var item in myTeam){
        Console.Write($" {item.animalName}  ");
    }
    Console.Write("\n  __________________________________________________________\n\n");
    if (wins == 2) {
        Console.Write("\n  CONGRADULATIONS! You have won Super Auto Pets! ");
        Console.ReadKey();
        myTeam.Clear();
        myTempTeam.Clear();
        roundNum = 1;
        wins = 0;
        health = 5;
        mainMenu();
    } else if ( health == 0) {
        Console.Write("\n  Sorry, you have lost in Super Auto Pets. ");
        Console.ReadKey();
        myTeam.Clear();
        myTempTeam.Clear();
        enemyTeam.Clear();
        roundNum = 1;
        wins = 0;
        health = 5;
        mainMenu();
    } else {
        enemyTeam.Clear();
        firstTime = true;
        coins = 10;
        roundNum++;
        gameStore();
    }
}
#endregion

#region Functions To-Do List
// Functionality: 
// - Make a Main Menu 
//   - Start the game [Done]
//   - Add an animal database where user can view all the animals available [Done]
//   - Exit the game [Done]
// - Make a shop
//   - Add 3 - 5 random animals from the full animal list to the store. [Done 3 animals]
//     - Round 1-4 = 3 animals | 1 food [Done Animals]
//     - Round 5-8 = 4 animals | 2 food [Done Animals]
//     - Round 9 + = 5 animals | 2 food [Done Animals]
//   - Add coin system [Done]
//   - Add player's team with rearrangement feature [Done]
//   - Add re-roll store feature [Done]
//   - Add sell option [Done]
//   - Add freeze shop pets [To Do]
//   - Add food items
//   - Add Fight button [Done]
//   - Add levels for each animal
// - Make a battle phase
//   - Generate random enemy team [Done]
#endregion

