﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.MSTest.TestAdapter.Execution
{
    using System;
    using System.Security;
    using Microsoft.VisualStudio.TestPlatform.MSTest.TestAdapter.Helpers;
    using Microsoft.VisualStudio.TestPlatform.MSTest.TestAdapter.ObjectModel;

    internal class TestAssemblySettingsProvider : MarshalByRefObject
    {
        private ReflectHelper reflectHelper;

        public TestAssemblySettingsProvider()
            : this(new ReflectHelper())
        {
        }

        internal TestAssemblySettingsProvider(ReflectHelper reflectHelper)
        {
            this.reflectHelper = reflectHelper;
        }

        /// <summary>
        /// Returns object to be used for controlling lifetime, null means infinite lifetime.
        /// </summary>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        [SecurityCritical]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        internal TestAssemblySettings GetSettings(string source)
        {
            var testAssemblySettings = new TestAssemblySettings();

            // Load the source.
            var testAssembly = PlatformServiceProvider.Instance.FileOperations.LoadAssembly(source, isReflectionOnly: false);

            testAssemblySettings.ParallelLevel = this.reflectHelper.GetParallelizationLevel(testAssembly);

            if (testAssemblySettings.ParallelLevel == 0)
            {
                testAssemblySettings.ParallelLevel = Environment.ProcessorCount;
            }

            testAssemblySettings.ParallelMode = this.reflectHelper.GetParallelizationMode(testAssembly);

            testAssemblySettings.CanParallelizeAssembly = !this.reflectHelper.IsDoNotParallelizeSet(testAssembly);

            return testAssemblySettings;
        }
    }
}
