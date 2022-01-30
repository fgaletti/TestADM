using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace TestADM.ConsoleApp
{
    class Program
    {
        public class Share
        {
            public Share(int shares, double price)
            {
                NumOfShares = shares;
                PricePerShare = price;
            }
            public int NumOfShares { get; set; }
            public double PricePerShare { get; set; }
        }
       static  void Main(string[] args)
        {
            // list of shares 
            List<Share> shareList = new List<Share>
            {
                 new Share(23,2.5),
                 new Share(47,2.54),
                 new Share(45,2.56),
                 new Share(50,2.60),
            };

            // list of shares to allocate 
            SortedList<string, int> accountList = new SortedList<string, int>();

            // case 1
            accountList.Add("account1", 50);
            accountList.Add("account2", 53);
            accountList.Add("account3", 17);
            accountList.Add("account4", 45);

            // case 2
            //accountList.Add("account1", 115);
            //accountList.Add("account2", 50);

            // case 3
            //accountList.Add("account1", 165);

            IOrderedEnumerable<Share> sortedShareList = shareList.OrderBy(x => x.PricePerShare); //sort the list of shares by price

            if (accountList.Sum(s => s.Value) > sortedShareList.Sum(s => s.NumOfShares))
            {
                Console.WriteLine("The number of shares to allocate should not be greater that the available shares");
            }

            int pointer = 0; //  which specific row of the list we are on (shares list) 
            int remainingSharesAccount = 0;
            int reminingShares = 0; //shares in the list 
            int sharesAllocate = 0;

            foreach (var account in accountList)
            {
                var sharesAvailable = sortedShareList.Select((x, n) => new { x.NumOfShares, x.PricePerShare, n }).Where(n => n.n == pointer).First(); // get the 
                reminingShares = reminingShares != 0 ? reminingShares : sharesAvailable.NumOfShares; // number of shares per list

                do // Allocate shares 
                {
                    if (remainingSharesAccount > 0)
                    {
                        pointer++; // searh available shares in the new row
                        sharesAvailable = sortedShareList.Select((x, n) => new { x.NumOfShares, x.PricePerShare, n }).Where(n => n.n == pointer).First(); 
                        reminingShares = sharesAvailable.NumOfShares; // we are in a new item  so now all the shares are  available     
                    }

                    remainingSharesAccount = remainingSharesAccount == 0 ? account.Value : remainingSharesAccount; // set the remaining shares

                    sharesAllocate = remainingSharesAccount == 0 ? account.Value : remainingSharesAccount; // shares to allocate to the account; it could be either the remainig shares or the total assigned for the account

                    if (sharesAllocate >= reminingShares)
                    {
                        Console.WriteLine($"Account: {account.Key} , Quantity Assigned {reminingShares} , trade price: {sharesAvailable.PricePerShare}"); //  Assigne ALL remaining shares   
                        remainingSharesAccount = remainingSharesAccount - reminingShares; // remaining shares asigned to account minus the allocated shares
                        reminingShares = 0; //Reset the value,  all shares were assigned in this row  
                    }
                    else
                    {
                       // sharesAllocate = remainingSharesAccount == 0 ? account.Value : remainingSharesAccount; // shares to allocate; if there is remaining shares all should be allocates if not the shares to allocate 
                        Console.WriteLine($"Account: {account.Key} , Quantity Assigned {(sharesAllocate)} , trade price: {sharesAvailable.PricePerShare}"); 

                        reminingShares = sharesAvailable.NumOfShares - sharesAllocate;
                        remainingSharesAccount = 0; // reset the remaining shares per accout because all the shares have been allocated for this account
                    }
                } while (remainingSharesAccount != 0); // check if there are still shares to allocate for this account

                if (reminingShares == 0)
                    pointer++; // go to the next row, there are no more shares in this row
            }

            Console.ReadKey();
        }
     
    }
}
