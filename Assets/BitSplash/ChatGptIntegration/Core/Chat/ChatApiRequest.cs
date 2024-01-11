using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BitSplash.AI.GPT
{
	/// <summary>
	/// this is an api request class as described in this article :https://platform.openai.com/docs/api-reference/chat/create
	/// </summary>
	[Serializable]
	public class ChatApiRequest
	{
		
		/// <summary>
		/// ID of the model to use
		/// </summary>
		public string model;
		/// <summary>
		/// The messages to generate chat completions for
		/// </summary>
		public ChatApiMessage[] messages;

		/// <summary>
		/// The maximum number of tokens allowed for the generated answer. By default, the number of tokens the model can return will be (4096 - prompt tokens).
		/// </summary>
		public int max_tokens;
		/// <summary>
		/// What sampling temperature to use, between 0 and 2. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic.
		///We generally recommend altering this or top_p but not both.
		/// </summary>
		public double temperature;
		/// <summary>
		/// An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered.
		///We generally recommend altering this or temperature but not both
		/// </summary>
		public double top_p;
		/// <summary>
		/// Number between -2.0 and 2.0. Positive values penalize new tokens based on whether they appear in the text so far, increasing the model's likelihood to talk about new topics.
		/// </summary>
		public double presence_penalty;
		/// <summary>
		/// Number between -2.0 and 2.0. Positive values penalize new tokens based on their existing frequency in the text so far, decreasing the model's likelihood to repeat the same line verbatim.
		/// </summary>
		public double frequency_penalty;
		/// <summary>
		/// How many chat completion choices to generate for each input message.
		/// </summary>
		public int n;
		/// <summary>
		/// 
		/// </summary>
		public bool stream = false;
		/// <summary>
		/// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
		/// </summary>
		//public string user;
	}
}
