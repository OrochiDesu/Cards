﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace KittenMafiaBlackJack
{
    public enum GameState
    {

        Initiate,                                                           // should only announce Player/s and Dealer 
        Shuffling,                                                          // shuffles deck
        Dealing,                                                            // dealing cards to player/s and dealer
        Starting,                                                           // begin game
        PlayersTurn,                                                        // waiting for player/s turn to end
        DealersTurn,
        Ending     
    }

    public class BlackJack
    {
        const int BlackJackDeal = 2;                                        // deal two cards for BlackJack
        const int BlackJackHit = 1;                                         // hit for one card

        KittenDeck deck;

        List<Player> players;                                               // keep a list of players
        int humanPlayerIndex;
        int dealerPlayerIndex;
        GameState currentGameState;

        public BlackJack()
        {
            deck = new BlackJackDeck();
            players = new List<Player> { new BlackJackPlayer(), new BlackJackDealer() };    // inits player and dealer into the list
            humanPlayerIndex = 0;                                                           
            dealerPlayerIndex = 1;                                                          // simple checks for "if" human or ai
            currentGameState = GameState.Initiate;                          
            StartGameLoop();
        }

        private Player GetPlayer(bool isHuman)
        {
            return isHuman ? players[humanPlayerIndex] : players[dealerPlayerIndex];        // if player "isHuman" == 0 / else == 1 bool for isHuman?(t/f) 
        }

        public void StartGameLoop()
        {
            while (true)
            {
                switch(currentGameState)
                {
                    case GameState.Initiate:
                        Console.WriteLine("\nPlease enter your name");
                        GetPlayer(true).SetPlayerName();
                        Console.WriteLine($"{GetPlayer(true).Name} I'm ready to play Kitten BlackJack! \nPlease press the return key...");
                        Console.ReadLine();
                        currentGameState = GameState.Shuffling;
                        break;
                    case GameState.Shuffling:
                        Console.WriteLine("Lets begin!");
                        Console.WriteLine("Everyday I'm shuffling..");
                        deck.Shuffle();
                        Console.ReadLine();
                        currentGameState = GameState.Dealing;
                        break;
                    case GameState.Dealing:
                        Console.WriteLine("Top deals, being dealt...");
                        GetPlayer(false).DealCardsToPlayer(deck.DealAmount(BlackJackDeal));
                        GetPlayer(true).DealCardsToPlayer(deck.DealAmount(BlackJackDeal));
                        Console.ReadLine();
                        currentGameState = GameState.Starting;
                        break;
                    case GameState.Starting:
                        StartGame();
                        currentGameState = GameState.PlayersTurn;
                        break;
                    case GameState.PlayersTurn:
                        ReadPlayerTurn(GetPlayer(true));
                        ProcessTurn();
                        if
                        else
                        break;
                        currentGameState = GameState.Ending;
                        break;
                    case GameState.Ending:
                        Console.Clear();
                        ShowHands();
                        RestartGame();
                        break;
                }
            }
        }

        private void StartGame()
        {
            var player = GetPlayer(true);
            var dealer = (BlackJackDealer)GetPlayer(false);
            Console.WriteLine($"{player.Name} you currently have:\n{player.HandToString()}\nDealer Has {dealer.PreviewHand()}\n{player.Name} would you like to [H]it or [S]tick?");
        }

        private void ReadPlayerTurn(Player player)
        {
            switch (KittenTools.GetInputString())
            {
                case "H":
                    ProcessTurn();
                    break;
                case "S":
                    ProcessStick();
                    break;
            }
        }

        private void ProcessTurn()
        {
            var player = (BlackJackPlayer)GetPlayer(true);
            var dealer = (BlackJackDealer)GetPlayer(false);

            Console.WriteLine($"\n{player.Name} would you like to [H]it or [S]tick");
            ReadPlayerTurn(player);

            if (player.HandCount() > 21 || player.HandCount() == 21 || KittenTools.GetInputString() == "s" && player.HandCount() < dealer.GetFaceVal(dealer.Hand[0]) || dealer.HandCount() > 21)
            {
                currentGameState = GameState.Ending;
            }
            else if (player.HandCount() < 21 && )
            {
                
            }

            if (dealer.HandCount() < 17)
            {
                Console.WriteLine($"\nDealer has {dealer.HandCount()}");
                Console.WriteLine(dealer.HandToString());
                Console.WriteLine("Dealer hits!, Press Enter...");
                Console.ReadLine();
                dealer.DealCardsToPlayer(deck.DealAmount(BlackJackHit));
            }
        }

        private void ProcessStick()
        {
            currentGameState = GameState.Ending;
        }

        private int[] GetPlayerHandTotal(Player player)
        {
            var firstTotals = player.Hand.Sum(x => x.CardValue[0]);

            if (firstTotals + (11 - 1) > 21 || !player.Hand.Any(x => x.CardValue.Length > 1))
                return new int[1] { firstTotals };

            return new int[2] { firstTotals, firstTotals + 10 };
        }
        
        public void ShowHands()
        {
            // need to utilise GetCardValue to compare both hands
            foreach (Player player in players)
            {
                Console.WriteLine($"\n{player.Name} you have { string.Join(" / ", GetPlayerHandTotal(player)) }");      // join takes all cards in hand and makes them string.
                Console.WriteLine(player.HandToString());
            }
            
            var human = GetPlayer(true);
            var dealer = GetPlayer(false);

            var winner = (human.HandCount() > dealer.HandCount() && human.HandCount() < 21) 
                ? human 
                : dealer;

            Console.WriteLine($"{winner.Name} Wins!");            

            Console.WriteLine("Play Again? [Y]es / [N]o");
        }

        private void RestartGame()
        {
            switch (KittenTools.GetInputString())
            {
                case "Y":
                    deck.ResetDeck();
                    GetPlayer(true).Hand.Clear();
                    GetPlayer(false).Hand.Clear();
                    currentGameState = GameState.Shuffling;
                    Console.Clear();
                    break;
                case "N":
                    Program.Exit();
                    break;
            }
        }        
    }
}
