"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
Object.defineProperty(exports, "HeadersArray", {
  enumerable: true,
  get: function () {
    return _types.HeadersArray;
  }
});
Object.defineProperty(exports, "Point", {
  enumerable: true,
  get: function () {
    return _types.Point;
  }
});
Object.defineProperty(exports, "Quad", {
  enumerable: true,
  get: function () {
    return _types.Quad;
  }
});
Object.defineProperty(exports, "Rect", {
  enumerable: true,
  get: function () {
    return _types.Rect;
  }
});
Object.defineProperty(exports, "Size", {
  enumerable: true,
  get: function () {
    return _types.Size;
  }
});
Object.defineProperty(exports, "TimeoutOptions", {
  enumerable: true,
  get: function () {
    return _types.TimeoutOptions;
  }
});
Object.defineProperty(exports, "URLMatch", {
  enumerable: true,
  get: function () {
    return _types.URLMatch;
  }
});
exports.kLifecycleEvents = void 0;

var _types = require("../common/types");

/**
 * Copyright 2018 Google Inc. All rights reserved.
 * Modifications copyright (c) Microsoft Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
const kLifecycleEvents = new Set(['load', 'domcontentloaded', 'networkidle', 'commit']);
exports.kLifecycleEvents = kLifecycleEvents;