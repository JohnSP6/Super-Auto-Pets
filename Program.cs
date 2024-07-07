// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;

List<Animals> animals = [];
List<Animals> myTeam = [];
List<Animals> tempStore = [];
List<Animals> enemyTeam = [];
List<Animals> myTempTeam = [];
int roundNum = 0;

Animals fish = new ("Fish", 2, 3);
Animals ant = new ("Ant", 3, 2);
Animals spider = new ("Spider", 3, 1);
Animals dog = new ("Dog", 2, 3);
Animals cat = new ("Cat", 3, 2);
Animals elephant = new ("Elephant", 3, 1);
Animals lion = new ("Lion", 2, 3);
Animals puma = new ("Puma", 3, 2);
Animals butterfly = new ("Butterfly", 3, 1);
animals.Add(fish);
animals.Add(ant);
animals.Add(spider);
animals.Add(dog);
animals.Add(cat);
animals.Add(elephant);
animals.Add(lion);
animals.Add(puma);
animals.Add(butterfly);


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
    //Coins are allocated to the player during shop phase.
    int coins = 9;
    //Refreshes the store until all coins are used.
    while (coins > 0) {
        Console.Clear();
        Console.Write($"\n                      Super Auto Pets \n  __________________________________________________________\n\n  Store: {coins} coins.\n\n" );
        //Randomly selects 3 animals from list to display in the store
        for (int j = 1; j < 4; j++){
            Random rnd = new Random();
            int randomAnimal  = rnd.Next(0, 9); 
            Console.WriteLine($"  {j}. {animals[randomAnimal].animalName} ");
            //Adds the 3 random animals to a new list so the user can select which to choose based on the UI.
            tempStore.Add(animals[randomAnimal]);
        }
        //Displays the animals in the player's team.
        Console.Write("\n  __________________________________________________________\n\n  Your Team:  ");
        foreach (var item in myTeam){
            Console.Write($" {item.animalName}  ");
        }
        Console.Write("\n  __________________________________________________________\n ");
        Console.Write("\n\n  Please select which animal to purchase? \n\n");
        //Prevent invalid input
        try {
            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice > animals.Count) {
                Console.Clear();
                Console.Write($"\n  Super Auto Pets \n\n  Sorry, invalid option. Please try again! \n  " );
                myTeam.Clear();
                Console.ReadKey();
                gameStore();
            } else {
                //Deducts coin value upon selecting an animal.
                coins -= 3;
                //The object/animal selected by the player is sent to the player's team list and the temp list is cleared. 
                myTeam.Add(tempStore[choice - 1]);
                tempStore.Clear();
            }
        } catch (Exception ex) {
            //Console.Clear();
            Console.Write($"\n  Super Auto Pets \n\n  Sorry, wrong input. Please try again!\n "+ ex.Message );
            myTeam.Clear();
            Console.ReadKey();
            gameStore();
        }
    }
    //Console.ReadKey();
    Console.Clear();
    Console.Write("\n                      Super Auto Pets \n  __________________________________________________________\n" );
    Console.Write("\n  Your Team:  ");
    foreach (var item in myTeam){
        Console.Write($" {item.animalName}  ");
    }
    Console.Write("\n  __________________________________________________________\n\n  Press Enter to move onto the Battle Stage! \n\n  ");
    Console.ReadKey();
    gameBattle();
}

void gameBattle(){
    //myTeam.Reverse();
    foreach (var item in myTeam){
        myTempTeam.Add(item);
    }
    for (int i = 1; i < 4; i++){
        Random rnd = new Random();
        int randomAnimal  = rnd.Next(0, 9); 
        enemyTeam.Add(animals[randomAnimal]);
    }
    while (myTeam.Count > 0 || enemyTeam.Count > 0) {
        Console.Clear();
        Console.Write($"\n                      Super Auto Pets \n  __________________________________________________________\n\n                      Battle Phase! \n" );
        Console.Write("\n  Player \n  ------\n");
        foreach (var item in myTeam){
            Console.Write($"  {item.animalName}  ");
        }
        //Test
        Console.Write($" Name: {myTeam.First().animalName} | Health: {myTeam.First().animalHealth} | Damage: {myTeam.First().animalDmg}");
        Console.Write("\n  __________________________________________________________\n\n  Enemy \n  ------\n");
        foreach (var item in enemyTeam){
            Console.Write($"  {item.animalName}  ");
        }
        //Test
        Console.Write($" Name: {enemyTeam.First().animalName} | Health: {enemyTeam.First().animalHealth} | Damage: {enemyTeam.First().animalDmg}");
        Console.Write("\n\n  __________________________________________________________\n\n  Battle Log \n  ----------\n");
        Console.Write($"\n\n The enemy's {enemyTeam.First().animalName} hurt your {myTeam.First().animalName} for {enemyTeam.First().animalDmg} damage.");
        myTeam.First().animalHealth = myTeam.First().animalHealth - enemyTeam.First().animalDmg;
        Console.ReadKey();
        Console.Write($"\n\n Your {myTeam.First().animalName} hurt the enemy's {enemyTeam.First().animalName} for {myTeam.First().animalDmg} damage.");
        enemyTeam.First().animalHealth = enemyTeam.First().animalHealth - myTeam.First().animalDmg;
        Console.ReadKey();
        if (myTeam.First().animalHealth <= 0 && enemyTeam.First().animalHealth <= 0) {
            Console.Write($"\n\n Both your {myTeam.First().animalName} and enemy's {enemyTeam.First().animalName} has died.");
            myTeam.Remove(myTeam.First());
            enemyTeam.Remove(enemyTeam.First());
            Console.ReadKey();
        } else if (enemyTeam.First().animalHealth <= 0) {
            Console.Write($"\n\n The enemy's {enemyTeam.First().animalName} has died.");
            enemyTeam.Remove(enemyTeam.First());
            Console.ReadKey();
        } else if (myTeam.First().animalHealth <= 0) {
            Console.Write($"\n  Your {myTeam.First().animalName} has died.");
            myTeam.Remove(myTeam.First());
            Console.ReadKey();
        }
        // var yourResult = myTeam.First().animalHealth - enemyTeam.First().animalDmg;
        // var theirResult = enemyTeam.First().animalHealth - myTeam.First().animalDmg;
        // if(yourResult <= 0){
        //     Console.Write($"\n\n The enemy's {enemyTeam.First().animalName} hurt your {myTeam.First().animalName} for {enemyTeam.First().animalDmg} damage.");
        //     myTeam.First().animalHealth = myTeam.First().animalHealth - enemyTeam.First().animalDmg;
        //     Console.Write($"\n  Your {myTeam.First().animalName} has died.");
        //     myTeam.Remove(myTeam.First());
        //     Console.ReadKey();
        // } else if (theirResult <= 0) {
        //     Console.Write($"\n\n Your {myTeam.First().animalName} hurt the enemy's {enemyTeam.First().animalName} for {myTeam.First().animalDmg} damage.");
        //     enemyTeam.First().animalHealth = enemyTeam.First().animalHealth - myTeam.First().animalDmg;
        //     Console.ReadKey();
        //     Console.Write($"\n\n The enemy's {enemyTeam.First().animalName} has died.");
        //     enemyTeam.Remove(enemyTeam.First());
        //     Console.ReadKey();
        // } else if (yourResult <= 0 && theirResult <= 0) {
        //     Console.Write($"\n\n The enemy's {enemyTeam.First().animalName} hurt your {myTeam.First().animalName} for {enemyTeam.First().animalDmg} damage.");
        //     myTeam.First().animalHealth = myTeam.First().animalHealth - enemyTeam.First().animalDmg;
        //     Console.Write($"\n\n Your {myTeam.First().animalName} hurt the enemy's {enemyTeam.First().animalName} for {myTeam.First().animalDmg} damage.");
        //     enemyTeam.First().animalHealth = enemyTeam.First().animalHealth - myTeam.First().animalDmg;
        //     Console.Write($"\n\n Both your {myTeam.First().animalName} and enemy's {enemyTeam.First().animalName} has died.");
        //     myTeam.Remove(myTeam.First());
        //     enemyTeam.Remove(enemyTeam.First());
        //     Console.ReadKey();
        // } else {
        //     Console.Write($"\n\n Your {myTeam.First().animalName} hurt the enemy's {enemyTeam.First().animalName} for {myTeam.First().animalDmg} damage.");
        //     enemyTeam.First().animalHealth = enemyTeam.First().animalHealth - myTeam.First().animalDmg;
        //     Console.ReadKey();
        //     Console.Write($"\n\n The enemy's {enemyTeam.First().animalName} hurt your {myTeam.First().animalName} for {enemyTeam.First().animalDmg} damage.");
        //     myTeam.First().animalHealth = myTeam.First().animalHealth - enemyTeam.First().animalDmg;
        //     Console.ReadKey();
        // }
    }
    if (myTeam.Count == 0) {
        Console.WriteLine($"\n  You Lose! " );
        Console.ReadKey();
    } else if (enemyTeam.Count == 0 && myTeam.Count == 0) {
        Console.WriteLine($"\n  It's a Draw! " );
        Console.ReadKey();
    } else {
        Console.WriteLine($"\n  You Win! " );
        Console.ReadKey();
    }
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
// - Make a battle phase
//   - Generate random enemy team
