﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ProtoBuf;
using Sandbox.Common.ObjectBuilders.VRageData;

namespace Sandbox.Common.ObjectBuilders.Definitions
{
    [ProtoContract]
    [MyObjectBuilderDefinition]
    public class MyObjectBuilder_CompoundBlockTemplateDefinition : MyObjectBuilder_DefinitionBase
    {
        // Rotation binding - defines allowed rotations of build type to referenced build type (eg. wall rotations to stairs). 
        // Referenced build type is in its default rotation (forward, up) and all specified rotations are referencing to it.
        // Same build type can also be referenced (it is always chekcked for allowed rotations from both sides - ie. placing block/placed block).
        [ProtoContract]
        public class CompoundBlockRotationBinding
        {
            [XmlAttribute]
            [ProtoMember(1)]
            public string BuildTypeReference;

            [XmlArrayItem("Rotation")]
            [ProtoMember(2)]
            public SerializableBlockOrientation[] Rotations = null;
        }

        [ProtoContract]
        public class CompoundBlockBinding
        {
            [XmlAttribute]
            [ProtoMember(1)]
            public string BuildType;

            [XmlAttribute]
            [ProtoMember(2), DefaultValue(false)]
            public bool Multiple = false;

            // Rotation binding - allowed rotations to referenced type of this build type.
            [XmlArrayItem("RotationBind")]
            [ProtoMember(3)]
            public CompoundBlockRotationBinding[] RotationBinds = null;
        }

        [XmlArrayItem("Binding")]
        [ProtoMember(1)]
        public CompoundBlockBinding[] Bindings;
    }
}
