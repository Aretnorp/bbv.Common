//-------------------------------------------------------------------------------
// <copyright file="MappingDefaultSpecification.cs" company="bbv Software Services AG">
//   Copyright (c) 2008-2011 bbv Software Services AG
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable ConvertToLambdaExpression

#pragma warning disable 169

namespace bbv.Common.MappingEventBrokerExtension
{
    using bbv.Common.EventBroker;

    using Machine.Specifications;

    using Moq;

    public class EventBrokerWithDefaultAutoMapperExtension
    {
        protected static Mock<IMapper> mapper;

        protected static Mock<IDestinationEventArgsTypeProvider> typeProvider;

        protected static IEventBroker eventBroker;

        protected static MappingEventBrokerExtension extension;

        protected static Publisher source;

        protected static Subscriber destination;

        Establish context = () =>
            {
                source = new Publisher();
                destination = new Subscriber();
                mapper = new Mock<IMapper>();
                typeProvider = new Mock<IDestinationEventArgsTypeProvider>();

                var mock = new Mock<MappingEventBrokerExtension>(mapper.Object, typeProvider.Object) { CallBase = true };
                extension = mock.Object;

                eventBroker = new EventBroker();
                eventBroker.AddMappingExtension(extension);

                eventBroker.Register(source);
                eventBroker.Register(destination);
            };
    }

    [Subject(Concern.MappingWithDefaultConvention)]
    public class when_publishing_topic_which_matches_default_convention_with_defined_mapping_with_destination_information : EventBrokerWithDefaultAutoMapperExtension
    {
        protected static string sourceEventDescription = "Source";

        Establish context = () =>
            {
                typeProvider.Setup(p => p.GetDestinationEventArgsType(Moq.It.IsAny<string>())).Returns(typeof(DestinationEventArgs));

                mapper.SetupMapping();
            };

        Because of = () =>
            {
                    source.Publish(sourceEventDescription);
            };

        Behaves_like<MappedEventFiredBehavior> event_argument_auto_mapper;
    }

    [Subject(Concern.MappingWithDefaultConvention)]
    public class when_publishing_topic_which_matches_default_convention_with_defined_mapping_but_without_destination_information : EventBrokerWithDefaultAutoMapperExtension
    {
        protected static bool wasCalled;

        protected static string sourceEventDescription = "Source";

        Establish context = () =>
            {
                typeProvider.Setup(p => p.GetDestinationEventArgsType(Moq.It.IsAny<string>())).Returns(() => null);

                mapper.SetupMapping();

                extension.SetMissingMappingAction((s, d, p, sender, eventArgs) => wasCalled = true);
            };

        Because of = () =>
            {
                source.Publish(sourceEventDescription);
            };

        Behaves_like<MappedEventNotFiredBehavior> not_an_event_argument_auto_mapper;
    }
}