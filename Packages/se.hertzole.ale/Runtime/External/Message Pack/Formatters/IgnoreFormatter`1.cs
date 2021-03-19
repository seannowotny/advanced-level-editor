﻿// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MessagePack.Formatters
{
    public sealed class IgnoreFormatter<T> : MessagePackFormatter<T>
    {
        public override void Serialize(ref MessagePackWriter writer, T value, MessagePackSerializerOptions options)
        {
            writer.WriteNil();
        }

        public override T Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            reader.Skip();
            return default(T);
        }
    }
}