//-------------------------------------------------------------------------------
// <copyright file="Publisher.cs" company="bbv Software Services AG">
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

namespace bbv.Common.MappingEventBrokerExtension
{
    using System;

    using bbv.Common.EventBroker;

    public class Publisher
    {
        [EventPublication(Topics.Publisher)]
        public event EventHandler<SourceEventArgs> Event;

        public void Publish(string source)
        {
            this.InvokeEvent(new SourceEventArgs(source));
        }

        private void InvokeEvent(SourceEventArgs e)
        {
            EventHandler<SourceEventArgs> handler = this.Event;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}