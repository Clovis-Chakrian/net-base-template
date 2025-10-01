using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ChaCha.Data.Resolvers;

public sealed class PrivateSetterContractResolver : DefaultContractResolver
{
  protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
  {
    var prop = base.CreateProperty(member, memberSerialization);
    if (!prop.Writable && member is PropertyInfo pi)
    {
      var hasPrivateSetter = pi.GetSetMethod(true) != null;
      prop.Writable = hasPrivateSetter;
    }
    return prop;
  }
}