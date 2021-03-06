"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.mimeTypeToComparator = void 0;

var _safe = _interopRequireDefault(require("colors/safe"));

var _jpegJs = _interopRequireDefault(require("jpeg-js"));

var _pixelmatch = _interopRequireDefault(require("pixelmatch"));

var _diff_match_patch = require("../third_party/diff_match_patch");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

/**
 * Copyright 2017 Google Inc. All rights reserved.
 * Modifications copyright (c) Microsoft Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
// Note: we require the pngjs version of pixelmatch to avoid version mismatches.
const {
  PNG
} = require(require.resolve('pngjs', {
  paths: [require.resolve('pixelmatch')]
}));

const mimeTypeToComparator = {
  'application/octet-string': compareBuffersOrStrings,
  'image/png': compareImages.bind(null, 'image/png'),
  'image/jpeg': compareImages.bind(null, 'image/jpeg'),
  'text/plain': compareText
};
exports.mimeTypeToComparator = mimeTypeToComparator;

function compareBuffersOrStrings(actualBuffer, expectedBuffer) {
  if (typeof actualBuffer === 'string') return compareText(actualBuffer, expectedBuffer);
  if (!actualBuffer || !(actualBuffer instanceof Buffer)) return {
    errorMessage: 'Actual result should be a Buffer or a string.'
  };
  if (Buffer.compare(actualBuffer, expectedBuffer)) return {
    errorMessage: 'Buffers differ'
  };
  return null;
}

function compareImages(mimeType, actualBuffer, expectedBuffer, options = {}) {
  var _options$threshold, _ref;

  if (!actualBuffer || !(actualBuffer instanceof Buffer)) return {
    errorMessage: 'Actual result should be a Buffer.'
  };
  const actual = mimeType === 'image/png' ? PNG.sync.read(actualBuffer) : _jpegJs.default.decode(actualBuffer);
  const expected = mimeType === 'image/png' ? PNG.sync.read(expectedBuffer) : _jpegJs.default.decode(expectedBuffer);

  if (expected.width !== actual.width || expected.height !== actual.height) {
    return {
      errorMessage: `Expected an image ${expected.width}px by ${expected.height}px, received ${actual.width}px by ${actual.height}px. `
    };
  }

  const diff = new PNG({
    width: expected.width,
    height: expected.height
  });
  const count = (0, _pixelmatch.default)(expected.data, actual.data, diff.data, expected.width, expected.height, {
    threshold: (_options$threshold = options.threshold) !== null && _options$threshold !== void 0 ? _options$threshold : 0.2
  });
  const maxDiffPixels1 = options.maxDiffPixels;
  const maxDiffPixels2 = options.maxDiffPixelRatio !== undefined ? expected.width * expected.height * options.maxDiffPixelRatio : undefined;
  let maxDiffPixels;
  if (maxDiffPixels1 !== undefined && maxDiffPixels2 !== undefined) maxDiffPixels = Math.min(maxDiffPixels1, maxDiffPixels2);else maxDiffPixels = (_ref = maxDiffPixels1 !== null && maxDiffPixels1 !== void 0 ? maxDiffPixels1 : maxDiffPixels2) !== null && _ref !== void 0 ? _ref : 0;
  const ratio = Math.ceil(count / (expected.width * expected.height) * 100) / 100;
  return count > maxDiffPixels ? {
    errorMessage: `${count} pixels (ratio ${ratio.toFixed(2)} of all image pixels) are different`,
    diff: PNG.sync.write(diff)
  } : null;
}

function compareText(actual, expectedBuffer) {
  if (typeof actual !== 'string') return {
    errorMessage: 'Actual result should be a string'
  };
  const expected = expectedBuffer.toString('utf-8');
  if (expected === actual) return null;
  const dmp = new _diff_match_patch.diff_match_patch();
  const d = dmp.diff_main(expected, actual);
  dmp.diff_cleanupSemantic(d);
  return {
    errorMessage: diff_prettyTerminal(d)
  };
}

function diff_prettyTerminal(diffs) {
  const html = [];

  for (let x = 0; x < diffs.length; x++) {
    const op = diffs[x][0]; // Operation (insert, delete, equal)

    const data = diffs[x][1]; // Text of change.

    const text = data;

    switch (op) {
      case _diff_match_patch.DIFF_INSERT:
        html[x] = _safe.default.green(text);
        break;

      case _diff_match_patch.DIFF_DELETE:
        html[x] = _safe.default.reset(_safe.default.strikethrough(_safe.default.red(text)));
        break;

      case _diff_match_patch.DIFF_EQUAL:
        html[x] = text;
        break;
    }
  }

  return html.join('');
}