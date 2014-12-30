using Newtonsoft.Json;

namespace TrafficSignal.Serializers {
	public class Json {
		public static string Serialize<T>(T type, JsonSerializerSettings settings = null) {
			var jsonString = JsonConvert.SerializeObject(type, typeof(T), settings);
			return jsonString;
		}
		public static T Deserialize<T>(string value, JsonSerializerSettings settings = null) {
			T result = (T)JsonConvert.DeserializeObject(value, typeof(T), settings);
			return result;
		}
	}
}
