using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parse;

namespace Quyd.Models
{
    public class BidList
    {
        List<Bid> bidList;

        public BidList()
        {
            bidList = null;
        }

        public BidList(List<Bid> bidList)
        {
            this.bidList = bidList;
        }

        public async Task<BidList> getBidListAsync(Post post)
        {
            if (bidList == null)
            {
                var query = from bid in ParseObject.GetQuery("Bid")
                            where bid.Get<ParseObject>("post").ObjectId == post.Object.ObjectId
                            select bid;
                try
                {
                    IEnumerable<ParseObject> bids_t = await query.FindAsync();

                    foreach(var bid in bids_t)
                    {
                        bidList.Add(new Bid(bid));
                    }
                }
                catch (ParseException ex)
                {
                    if (ex.Code == ParseException.ErrorCode.ObjectNotFound)
                    {
                        //no post found
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            return new BidList(bidList);
        }
    }

    public class Bid
    {
        ParseObject bid;

        ItemList storeBidItems = null;

        public Bid()
        {

        }

        public Bid(ParseObject bid)
        {
            this.bid = bid;
        }

        public async Task<Store> getStoreAsync()
        {
            return new Store(await bid.Get<ParseObject>("bidStore").FetchIfNeededAsync());
        }

        public async Task<ItemList> getStoreBidItems(ItemList userItems)
        {
            if (storeBidItems == null)
            {
                var store = await getStoreAsync();
                await storeBidItems.loadStoreItemsAsync(store, bid.CreatedAt, userItems);
            }
            return storeBidItems;
        }
    }
}
