using System;
public class Card
{
    public enum Suit
    {
        // 扑克花色分别为黑桃（Spade）、红桃（Heart）、方块（Diamond）、梅花（Club）
        Jokers = -1,
        Hearts = 2,
        Diamonds = 0,
        Clubs = 1,
        Spades = 3
    }
    
    public enum Rank
    {
     
        Two = 0,
        Three = 1,
        Four = 2,
        Five = 3,
        Six = 4,
        Seven = 5,
        Eight = 6,
        Nine = 7,
        Ten = 8,
        Jack = 9,
        Queen = 10,
        King = 11,
        Ace = 12,
        // 13 27
        Joker1 = 13,
        Joker2 = 27
    }
    
    public Suit suit;
    public Rank rank;
    public Card(Suit suit, Rank rank)
    {
        this.suit = suit;
        this.rank = rank;
    }
    public override string ToString()
    {
        return string.Format("{0} of {1}", rank, suit);
    }
    public int indexOfSprite()
    { 
        if(this.suit==Suit.Jokers)
            return (int)rank;
        return (int)rank+14*(int)suit;
    }
    public bool IsJoker() {
        return (this.suit==Suit.Jokers && (this.rank == Rank.Joker1||this.rank == Rank.Joker2));
    }
    public int GetCardRank()
    {
        if(this.suit==Suit.Jokers)
            if(this.rank==Rank.Joker1)
                return 16;
            else
                return 17;
        
        if(this.rank==Rank.Two)
            return 15;
        return (int)this.rank+2;
    }

    public string soundName{
        get{
            switch (this.rank)
            {
                case Rank.Two:
                    return "2";
                case Rank.Three:
                    return "3";
                case Rank.Four:
                    return "4";
                case Rank.Five:
                    return "5";
                case Rank.Six:
                    return "6";
                case Rank.Seven:
                    return "7";
                case Rank.Eight:
                    return "8";
                case Rank.Nine:
                    return "9";
                case Rank.Ten:
                    return "10";
                 case Rank.Jack:
                    return "J";
                case Rank.Queen:
                    return "Q";
                case Rank.King:
                    return "K";
                case Rank.Ace:
                    return "A";
                case Rank.Joker1:
                    return "sking";
                case Rank.Joker2:
                    return "bking";
            }
            return "";
        }
    }

    public static bool operator ==(Card obj1, Card obj2) 
    {
        if (object.ReferenceEquals(obj1, obj2))
        {
            return true;
        }

        if (object.ReferenceEquals(obj1, null) || object.ReferenceEquals(obj2, null))
        {
            return false;
        }
        // 比较运算符
        return obj1.suit == obj2.suit && obj1.rank == obj2.rank;
    }
    public static  bool operator !=(Card obj1, Card obj2) 
    {
        
        // 比较运算符
        return !(obj1== obj2);
    }
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Card otherCard = (Card)obj;

        return this.suit == otherCard.suit && this.rank == otherCard.rank;
    }
}