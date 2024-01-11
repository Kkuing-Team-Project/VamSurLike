using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitSplash.AI.GPT
{
    public class ModelNames
    {
        public const string ModelGPT_3_5_Turbo = "gpt-3.5-turbo";
        public const string ModelGPT_4 = "gpt-4";
        public const string ModelGPT_3_5_Turbo_16K = "gpt-3.5-turbo-16k";
        public const string ModelGPT_4_32K = "gpt-4-32k";
        public static string GetModelName(ChatModels model)
        {
            if (model == ChatModels.GPT_3_5_TURBO)
                return ModelGPT_3_5_Turbo;
            if (model == ChatModels.GPT_3_5_TURBO_16K)
                return ModelGPT_3_5_Turbo_16K;
            if (model == ChatModels.GPT_4_32K)
                return ModelGPT_4_32K;
            return ModelGPT_4;
        }
    }
}
