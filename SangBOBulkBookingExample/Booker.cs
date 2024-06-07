using SangBO;
using System;

namespace SangBOBulkBookingExample
{
	public class Booker
	{
		private readonly UserInfo _ui;

		public Booker(UserInfo ui)
		{
			_ui = ui ?? throw new ArgumentNullException(nameof(ui));
		}

		/// <summary>
		/// Book many Orders and many Ads
		/// </summary>
		public void Book()
		{
			AdSaveHelper hlp = new AdSaveHelper(_ui, true); //this server both as caching mechanism to avoid loading same objects repeatedly and it also serves for "after save" method when booking multiple ads into an Order

			AdCommon ad = null;

			for (int i = 0; i < 10; i++) //lets say we are booking 10 orders in total
			{
				Order o = new Order(_ui);
				o.Knowledgebase = hlp; //in the future, this will be possible to set via Order constructor
				//now set order properties however you want

				o.Save(OrderSaveAction.FormSave);
		
				for (int j = 0; j < 10; j++) //lets say we are booking 10 ads into this order
				{
					ad = new AdDisplay(_ui, o.OrderID, false); //in the future, we'll implement a constructor to accept the Order object which you already have, instead of loading the Order in the Ad constructor
					ad.AdSaveHelper = hlp;
					//now set your ad properties however you want

					ad.SaveAd(AdSaveAction.FormSave, 12345);
				}
			}

			//once you are done booking ads, it is required to call "after save" method, which is not called automatically when the AdSaveHelper is set to bulk mode
			ad?.AfterSave(AdSaveAction.FormSave);

			//if you want to reuse the AdSaveHelper and its knowledgebase further, then you should flush the ads collections
			hlp.ClearAds();
		}
	}
}