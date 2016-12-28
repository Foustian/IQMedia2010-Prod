/*
 *
 */
package urlshorten.events
{
	import flash.events.Event;
	
	/**
	 * 
	 */
	public class URLShortenEvent extends Event
	{
		// 
		public static const ON_URL_SHORTED:String = "onUrlShorter";

		// 
		public static const ON_ERROR:String = "onError";

		// 
		public var url:String;

		// 
		public var service:String;
		
		// 
		public var data:Object;
		
		/**
		 * URLShorten Event
		 * 
		 * @param	type		
		 * @param	bubbles		
		 * @param	cancelable	
		 */
		public function URLShortenEvent(type:String, bubbles:Boolean=false, cancelable:Boolean=false) 
		{
			super (type, bubbles, cancelable);			
		}
	}
}