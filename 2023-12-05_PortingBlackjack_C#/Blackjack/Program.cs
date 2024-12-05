/*
 * Name: Kaci Craycraft
 * South Hills Username: kcraycraft45
 */

using System.Diagnostics;

namespace Blackjack
{
    public class Program
    {
        public const int STARTING_TOKENS = 100;
        public const int DEALER_STAND = 17;

        public static readonly String[] CARD_NAMES =
        {
            "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King"
        };
        public static readonly String[] CARD_SUITS =
        {
            "Clubs", "Spades", "Hearts", "Diamonds"
        };
        public static List<String> cards = new List<String>();
        public static int tokens = STARTING_TOKENS;
        public static void Main()
        {
            int tokensBet;
            int dealersCard;
            int playerScore;
            int dealerScore;
            CreateDeck();//Creates 52 cards with assigned suits and names.
            List<String> cardsPlaceholder = cards;//This keeps cards the same, but allows me to use my functions while changing cardsPlaceholder.
            //It is a disgusting workaround, but it gets the job done
            Console.WriteLine("---Blackjack___");//Anounce the game
            tokensBet = GetBet();//returns -1 if user wants to quit, otherwise returns their bet
            while (tokens > 0 && tokensBet != -1)//Keep looping if they have money and don't want to quit
            {
                dealersCard = 0;
                Thread.Sleep(2000);
                Console.Clear();
                Console.WriteLine("---Blackjack___");
                Console.WriteLine("Tokens: " + tokens);
                Console.WriteLine("Your bet: " + tokensBet);


                (cardsPlaceholder, dealersCard) = GenerateCard("The Dealer's first card is a", cardsPlaceholder, false);

                //Returns the score value of the card the dealer got
                Console.Write(Environment.NewLine);

                if (dealersCard == 1)
                {//If the dealer gets an ace, they will choose 11 points as its value
                    Console.WriteLine("Because the dealer's first card was an ace, they chose to receive 11 points.");
                    dealersCard = 11;
                }
                Debug.Assert(dealersCard != 0);
                Thread.Sleep(1000);
                (cardsPlaceholder, playerScore) = PlayerPlay(cardsPlaceholder);
                
                //I probably could have turned these three lines into a method because I use them a lot, but I am simply not doing that for my sanity.

                (cardsPlaceholder, dealerScore) = DealerPlay(dealersCard, cardsPlaceholder);
                
                
                //Returns the turn score of the dealer
                DetermineWinner(playerScore, dealerScore, tokensBet);//Adds/Subtracts bet from tokens and tells the user who won the round
                tokensBet = GetBet();//Start again
            }
            //Outside of while loop, they chose not to play or ran out of tokens to bet
            ExitPrint();

        }
        public static void CreateDeck()//Creates 52 cards with assigned suits and names.
        {
            cards.Clear();
            for (int i = 0; i < CARD_SUITS.Length; i++)
            {
                for (int j = 0; j < CARD_NAMES.Length; j++)
                {
                    cards.Add($"{CARD_NAMES[j]} of {CARD_SUITS[i]}");
                }
            }
        }
        public static int GetBet()
        {
            String input;

            if (tokens == 0) return 0;//If user has nothing to bet, they cannot bet

            Console.Write(Environment.NewLine);

            while (true)//keep looping until we decide to break
            {
                Console.WriteLine($"You have {tokens} tokens.");//Tell the user how many tokens they have
                Console.WriteLine("You can bet up to your token amount or type in \"q\" to quit.");//Prompt a bet or an option to quit
                Thread.Sleep(1000);
                Console.Write("What would you like to bet/do? ");
                input = Console.ReadLine();

                if (int.TryParse(input, out int bet))//If input is an integer, assign it to variable 'bet' and return bet to main
                {
                    if (bet <= 0)
                    {//Cannot bet less than 1 token
                        Console.WriteLine("You must bet at least 1 token.");
                    }
                    else if (bet > tokens)
                    {//Cannot bet more than you have
                        Console.WriteLine("You cannot bet more tokens than you currently have.");
                    }
                    else
                    {//Bet is valid otherwise.
                        return bet;

                    }
                }
                else
                {//if input is not an integer
                    if (input.Equals("q", StringComparison.OrdinalIgnoreCase))
                    {//If user wants to quit, return -1
                        return -1;
                    }
                    else
                    {//Otherwise, loop until they make a valid decision
                        Console.WriteLine("Invalid Input.");
                    }
                }
            }
        }

        public static (List<String>, int) GenerateCard(String message, List<String> cardsPlaceholder, bool playerCard = true)//Returns the value of their card
        {//Takes in a message and a boolean.  Boolean is true unless stated otherwise
            int cardIndex;
            String cardTitle;
            int cardPointValue = 0;
            int cardNotAnAceVal;

            Random rand = new Random();
            cardIndex = rand.Next(0, cardsPlaceholder.Count);//pick a random card from the deck using the index
                                                             //cards is a list of all 52 cards.

            cardTitle = cardsPlaceholder[cardIndex];//card's title is self-explanatory
            
            
            if ("a".Contains(cardTitle.ToLower()[0]))
            {
                if (playerCard)
                {
                    cardPointValue = AceDecision();
                    
                }
            }
            else if ("e".Contains(cardTitle.ToLower()[0]))
            {
                cardPointValue = 8;
            }
            else if ("n".Contains(cardTitle.ToLower()[0]))
            {
                cardPointValue = 9;
            }
            else if ("j".Contains(cardTitle.ToLower()[0]) || "q".Contains(cardTitle.ToLower()[0]) || "k".Contains(cardTitle.ToLower()[0]))
            {
                cardPointValue = 10;
            }
            else if ("t".Contains(cardTitle.ToLower()[0]))
            {
                if ("w".Contains(cardTitle.ToLower()[1]))
                {
                    cardPointValue = 2;
                }
                else if ("h".Contains(cardTitle.ToLower()[1]))
                {
                    cardPointValue = 3;
                }
                else
                {
                    cardPointValue = 10;
                }
            }
            else if ("f".Contains(cardTitle.ToLower()[0]))
            {
                if ("o".Contains(cardTitle.ToLower()[1]))
                {
                    cardPointValue = 4;
                }
                else
                {
                    cardPointValue = 5;
                }
            }
            else
            {
                if ("i".Contains(cardTitle.ToLower()[1]))
                {
                    cardPointValue = 6;
                }
                else { cardPointValue = 7; }
            }
            cardsPlaceholder.Remove(cardTitle);

            if ("aeiou".Contains(cardTitle.ToLower()[0]))
            {
                cardTitle = "n " + cardTitle;//Correct grammar function
            }
            else
            {
                cardTitle = " " + cardTitle;//""
            }

            

            //How much is the card worth?
            //( Card index / 13 remainder + 1 ) = the card's value
            Console.WriteLine(message + cardTitle);//Tell the user what card they got


            
            return (cardsPlaceholder, cardPointValue);


        }
        public static int AceDecision()//Ace results in a choice of either 1 pt or 11 pts.
        {
            int value;

            Console.WriteLine("Because your card was an ace, you can choose for it to be worth either 1 or 11 points.");
            Console.Write("Enter \"1\" or \"11\" for how many points your card should be worth: ");
            value = int.Parse(Console.ReadLine());

            while (value != 1 && value != 11)//Input validation
            {
                Console.WriteLine("Invalid Entry!");
                Console.WriteLine("Enter \"1\" or \"11\" for how many points your card should be worth: ");
                value = int.Parse(Console.ReadLine());
            }
            return value;//return the choice
        }
        public static (List<String>, int) PlayerPlay(List<String> filler)//Returns the score of player's turn
        {
            bool quit = false;
            int cardTotal = 0;
            int cardVal;
            String decision;

            Thread.Sleep(2000);
            Console.Clear();

            (filler, cardVal) = GenerateCard("Your first card is a", filler);
            cardTotal += cardVal;//Add the points of their first card
            

            Thread.Sleep(1000);

            (filler, cardVal) = GenerateCard("Your second card is a", filler);
            cardTotal += cardVal;//Add the points of their second card.
            
            //^^I hate this hahahahaha

            Thread.Sleep(1000);

            while (cardTotal < 21 && !quit)//If they have not busted and don't want to stand, loop
            {
                Console.WriteLine("The point value of your cards is " + cardTotal + ".");
                Console.WriteLine("Would you like to hit or stand? ");
                Console.Write("Enter either \"hit\" or \"stand\": ");//Prompt to hit or stand
                decision = Console.ReadLine().ToLower();//puts their decision in lowercase...
                //I just wanted to have code that shows both ways to compare string
                while (decision != "hit" && decision != "stand")//for comparison here.
                {//Input validation
                    Console.WriteLine("Invalid input!");
                    Console.Write("Enter either \"hit\" or \"stand\": ");
                    decision = Console.ReadLine();
                }
                if (decision.Equals("stand", StringComparison.OrdinalIgnoreCase))
                {//User wishes to stand
                    quit = true;
                }
                else
                {//User wishes to hit
                    (filler, cardVal) = GenerateCard("Your next card is a", filler);
                    cardTotal += cardVal;
                    

                }
            }
            //Outside of the loop, they have either busted, or stood.
            if (cardTotal == 21)
            {//They scored blackjack!
                Console.WriteLine("BLACKJACK");
            }
            else if (cardTotal < 21)
            {//They did not bust
                Console.WriteLine("You stood with " + cardTotal + " points.");
            }
            
            return (filler, cardTotal);
        }
        public static (List<String>, int) DealerPlay(int dealersFirstCard, List<String> cardsPlaceholder)//Returns the dealers score turn
        {//Takes an argument of the dealers first card
            int dealerPoints;
            int nextCard;


            Thread.Sleep(2000);
            Console.Clear();

            dealerPoints = dealersFirstCard;
            Console.WriteLine("\nThe dealer starts with " + dealerPoints + " because of their first card.");
            Console.WriteLine("The dealer will hit until they reach at least " + DEALER_STAND + " points.");//Dealer chooses to stand at 17
            while (dealerPoints < DEALER_STAND)
            {//Keep looping as long as dealer doesn't have >= 17 points
                (cardsPlaceholder, nextCard) = GenerateCard("The dealer's next card is a", cardsPlaceholder, false);
                
                //Scores their next card
                if (nextCard == 1)
                {//Dealer drew an ace
                    if (dealerPoints <= 10)
                    {//If they have less than or equal to 10 points, they choose for their ace to be worth 11 points
                        Console.WriteLine("The dealer chose to add 11 points.");
                        dealerPoints += 11;
                    }
                    else
                    {//Otherwise, they choose for their ace to be worth 1 point.
                        Console.WriteLine("Because adding 11 to the dealer's points would make them go bust, they chose to add 1 point.");
                        dealerPoints += 1;
                    }
                }
                else
                {//Dealer did not draw an ace
                    dealerPoints += nextCard;//Score their card and add to their turn total
                }
                Console.WriteLine("The dealer's current point total is " + dealerPoints + " points.");

                Thread.Sleep(1000);
            }
            //Outside of while loop, dealer has chosen to stand.
            if (dealerPoints < 21)
            {
                Console.WriteLine("The dealer stood with " + dealerPoints + " points.");
            }
            
            return (cardsPlaceholder, dealerPoints);
        }
        public static void DetermineWinner(int playerScore, int dealerScore, int betTokens)//Adds/Subtracts your score to/from your total and tells the user who won.
        {
            Console.WriteLine("\n");

            if (playerScore > 21 && dealerScore > 21)
            {
                Console.WriteLine("Both you and the dealer busted.");
            }
            else if (playerScore == dealerScore)
            {
                Console.WriteLine("You tied with the dealer.");
            }
            else if (playerScore > 21)
            {
                Console.WriteLine("You busted");
                tokens -= betTokens;
                Console.WriteLine("You lost " + betTokens + " tokens.");
            }
            else if (dealerScore > 21)
            {
                Console.WriteLine("The dealer busted, so you win.");
                tokens += betTokens;
                Console.WriteLine("You won " + betTokens + " tokens.");
            }
            else if (playerScore > dealerScore)
            {
                Console.WriteLine("You win this hand.");
                tokens += betTokens;
                Console.WriteLine("You won " + betTokens + " tokens.");
            }
            else
            {
                Console.WriteLine("You lost this hand.");
                tokens -= betTokens;
                Console.WriteLine("You lost " + betTokens + " tokens.");
            }
        }
        public static void ExitPrint()//Tells the user how they ended
        {
            int pointDifference;

            pointDifference = Math.Abs(tokens - STARTING_TOKENS);
            if (tokens == 0)
            {
                Console.WriteLine("Sorry, you ran out of tokens.");
            }
            else if (tokens > STARTING_TOKENS)
            {
                Console.WriteLine("You made a profit of " + pointDifference + " tokens.");
            }
            else if (tokens < STARTING_TOKENS)
            {
                Console.WriteLine("Seems like you're " + pointDifference + " tokens lighter. Better luck next time.");
            }
            else
            {
                Console.WriteLine("You broke even.");
            }
            Console.WriteLine("Come back again!!");
        }
    }
}

