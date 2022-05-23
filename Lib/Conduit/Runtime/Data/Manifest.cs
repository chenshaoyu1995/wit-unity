﻿/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Meta.Conduit
{
    /// <summary>
    /// The manifest is the core artifact generated by Conduit that contains the relevant information about the app.
    /// This information can be used to train the backend or dispatch incoming requests to methods.
    /// </summary>
    internal class Manifest
    {
        /// <summary>
        /// The App ID.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The version of the Manifest format
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// A human friendly name for the application/domain.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// List of relevant entities.
        /// </summary>
        public List<ManifestEntity> Entities { get; set; }

        /// <summary>
        /// List of relevant actions (methods).
        /// </summary>
        public List<ManifestAction> Actions { get; set; }

        /// <summary>
        /// Maps action IDs (intents) to CLR methods.
        /// </summary>
        private readonly Dictionary<string, MethodInfo> methodLookup = new Dictionary<string, MethodInfo>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Processes all actions in the manifest and associate them with the methods they should invoke.
        /// </summary>
        public void ResolveActions()
        {
            foreach (var action in this.Actions)
            {
                var lastPeriod = action.ID.LastIndexOf('.');
                var typeName = action.ID.Substring(0, lastPeriod);
                var qualifiedTypeName = $"{typeName},{action.Assembly}";
                var method = action.ID.Substring(lastPeriod + 1);

                // TODO: Support instance resolution
                var isStatic = true;

                if (isStatic)
                {
                    var targetType = Type.GetType(qualifiedTypeName);
                    var targetMethod = targetType.GetMethod(method);
                    if (targetMethod != null)
                    {
                        this.methodLookup.Add(action.Name, targetMethod);
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if the manifest contains the specified action.
        /// </summary>
        /// <param name="action"></param>
        /// <returns>True if the action exists, false otherwise.</returns>
        public bool ContainsAction(string @action)
        {
            return this.methodLookup.ContainsKey(action);
        }

        /// <summary>
        /// Returns the info of the method corresponding to the specified action ID.
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public MethodInfo GetMethod(string actionId)
        {
            return this.methodLookup[actionId];
        }
    }
}