//-------------------------------------------------------------------------------
// <copyright file="LoadExtensionConfigurationSection.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Configuration.Internals
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Default ILoadExtensionConfigurationSection
    /// </summary>
    public class LoadExtensionConfigurationSection : ILoadExtensionConfigurationSection
    {
        private readonly Func<string, ExtensionConfigurationSection> sectionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadExtensionConfigurationSection"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public LoadExtensionConfigurationSection(IExtension extension)
        {
            var loader = extension as ILoadExtensionConfigurationSection;
            this.sectionProvider =
                section =>
                loader != null
                    ? loader.GetSection(section)
                    : (ExtensionConfigurationSection)ConfigurationManager.GetSection(section);
        }

        /// <summary>
        /// Gets a configuration section according to the specified name.
        /// </summary>
        /// <param name="sectionName">The section name.</param>
        /// <returns>
        /// The section.
        /// </returns>
        public ExtensionConfigurationSection GetSection(string sectionName)
        {
            return this.sectionProvider(sectionName);
        }
    }
}