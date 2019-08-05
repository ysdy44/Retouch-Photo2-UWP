using Newtonsoft.Json;
using System.Numerics;

namespace Retouch_Photo2.Adjustments
{
    /// <summary>
    /// 通过属性标签自定义JSON序列化  https://www.cnblogs.com/wuyifu/p/3299788.html
    ///
    /// <see cref="JsonObjectAttribute"/>：类修饰标签，用于控制类如何被序列化为一个json对象
    /// <see cref="JsonArrayAttribute"/>：集合修饰标签，用于控制集合如何被序列化为一个json对象
    /// <see cref="JsonPropertyAttribute"/>：域和属性修饰标签，用于控制它们如何被序列化为一个json对象中的属性
    /// <see cref="JsonConverterAttribute"/>：类，域，属性修饰标签，用于指定序列化期间的转换器
    /// 
    /// </summary>
    [JsonObjectAttribute(MemberSerialization.OptIn)]
    class JsonAttribute
    {
        /// <summary>
        /// Instructs the Newtonsoft.Json.Json Serializer how to serialize the collection.
        /// 指导 Newtonsoft.Json.Json 序列化器如何序列化集合。
        /// </summary>
        Newtonsoft.Json.JsonArrayAttribute a = new Newtonsoft.Json.JsonArrayAttribute();

        /// <summary>
        /// Instructs the Newtonsoft.Json.Json Serializer to use the specified constructor  when deserializing that object.
        /// 指示 Newtonsoft.Json.Json 系列化程序在反序列化该对象时使用指定的构造函数。
        /// </summary>
        Newtonsoft.Json.JsonConstructorAttribute b = new Newtonsoft.Json.JsonConstructorAttribute();

        /// <summary>
        /// Instructs the Newtonsoft.Json.Json Serializer how to serialize the object.
        /// 指导 Newtonsoft.Json.Json 序列化器如何序列化对象。
        /// </summary>
        //Newtonsoft.Json.JsonContainerAttribute c = new Newtonsoft.Json.JsonContainerAttribute();

        /// <summary>
        /// Instructs the Newtonsoft.Json.Json Serializer to use the specified Newtonsoft.Json.Json Converter when serializing the member or class.
        /// 指示 Newtonsoft.Json.Json 序列化器在序列化成员或类时使用指定的 Newtonsoft.Json.Json 转换器。
        /// </summary>
        Newtonsoft.Json.JsonConverterAttribute d = new Newtonsoft.Json.JsonConverterAttribute(typeof(int));

        /// <summary>
        /// Instructs the Newtonsoft.Json.Json Serializer how to serialize the collection.
        /// 指导 Newtonsoft.Json.Json 序列化器如何序列化集合。
        /// </summary>
        Newtonsoft.Json.JsonDictionaryAttribute e =new Newtonsoft.Json.JsonDictionaryAttribute();

        /// <summary>
        /// Instructs the Newtonsoft.Json.Json Serializer to deserialize properties with no matching class member into the specified collection and write values during serialization.
        /// 指示 Newtonsoft.Json.Json 系列化程序将没有匹配类成员的属性反序列化到指定的集合中,并在序列化期间写入值。
        /// </summary>
        Newtonsoft.Json.JsonExtensionDataAttribute f = new Newtonsoft.Json.JsonExtensionDataAttribute();

        /// <summary>
        /// Instructs the Newtonsoft.Json.Json Serializer not to serialize the public field or public read/write property value.
        /// 指示 Newtonsoft.Json.Json 序列化程序不要序列化公共字段或公共读/写属性值。
        /// </summary>
        Newtonsoft.Json.JsonIgnoreAttribute g = new Newtonsoft.Json.JsonIgnoreAttribute();

        /// <summary>
        /// Instructs the Newtonsoft.Json.Json Serializer how to serialize the object.
        /// 指导 Newtonsoft.Json.Json 序列化器如何序列化对象。
        /// 
        /// 
        /// <see cref="MemberSerialization"/>
        /// 
        /// 默认情况下,所有公共成员都进行序列化。成员可以使用 Newtonsoft.Json.JsonIgnore 属性排除
        /// 或 System.NonSerializedAttribute。这是默认成员序列化模式。
        /// <see cref="MemberSerialization.OptOut"/> = 0
        ///
        /// 仅标记为 Newtonsoft.Json.JsonProperty 属性或 System.Runtime.Serialization.DataMemberAttribute 的成员
        /// 序列化。此成员序列化模式也可以通过标记
        /// 类与System.Runtime.Serialization.DataContractAttribute.
        /// <see cref="MemberSerialization.OptIn"/> = 1
        /// 
        /// 所有公共字段和私有字段都序列化。成员可以使用 Newtonsoft.Json.JsonIgnore 属性排除
        /// 或 System.NonSerializedAttribute。此成员序列化模式也可以
        /// 通过使用 System.SerializableAttribute 并设置"IgnoreSerializableAttribute"来设置
        /// 在 Newtonsoft.Json.序列化.默认合同解析器为假。
        /// <see cref="MemberSerialization.Fields"/> = 2
        /// 
        /// </summary>
        Newtonsoft.Json.JsonObjectAttribute h = new Newtonsoft.Json.JsonObjectAttribute(MemberSerialization.Fields);

        /// <summary>
        /// Instructs the Newtonsoft.Json.Json Serializer to always serialize the member with the specified name.
        /// 指示 Newtonsoft.Json.Json 序列化器始终使用指定名称序列化成员。
        /// </summary>
        Newtonsoft.Json.JsonPropertyAttribute i = new Newtonsoft.Json.JsonPropertyAttribute(nameof(Vector2.Zero));

        /// <summary>
        /// Instructs the Newtonsoft.Json.Json Serializer to always serialize the member, and to require that the member has a value.
        /// 指示 Newtonsoft.Json.Json 序列化器始终序列化成员,并要求成员具有值。
        /// </summary>
        Newtonsoft.Json.JsonRequiredAttribute j = new Newtonsoft.Json.JsonRequiredAttribute();

    }
}
