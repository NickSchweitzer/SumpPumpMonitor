/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		        if (installedModules[moduleId]) return installedModules[moduleId].exports;
/******/
/******/ 		        var module = installedModules[moduleId] = {
/******/ 		          i: moduleId,
/******/ 		          l: false,
/******/ 		          exports: {}
/******/ 		        };
/******/
/******/ 		        if (!modules[moduleId] && typeof moduleId === 'string') {
/******/ 		          var newModuleId;
/******/ 		          if (modules[newModuleId = moduleId + '.js'] || modules[newModuleId = moduleId + '.ts']) {
/******/ 		            moduleId = newModuleId;
/******/
/******/ 		            installedModules[moduleId] = module;
/******/ 		          }
/******/ 		        }
/******/
/******/ 		        modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		        module.l = true;
/******/
/******/ 		        return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// identity function for calling harmory imports with the correct context
/******/ 	__webpack_require__.i = function(value) { return value; };
/******/
/******/ 	// define getter function for harmory exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		Object.defineProperty(exports, name, {
/******/ 			configurable: false,
/******/ 			enumerable: true,
/******/ 			get: getter
/******/ 		});
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "/dist";
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 105);
/******/ })
/************************************************************************/
/******/ ({

/***/ 0:
/***/ function(module, exports) {

module.exports = vendor_37dda14c79b8ce5aba71;

/***/ },

/***/ 1:
/***/ function(module, exports, __webpack_require__) {

module.exports = (__webpack_require__(0))(29);

/***/ },

/***/ 105:
/***/ function(module, exports, __webpack_require__) {

__webpack_require__("aurelia-config");
__webpack_require__("aurelia-form");
module.exports = __webpack_require__(4);


/***/ },

/***/ 16:
/***/ function(module, exports, __webpack_require__) {

"use strict";
/* eslint-disable max-lines */

'use strict';

var extend      = __webpack_require__(9);
var expand      = __webpack_require__(32);
var Utils       = __webpack_require__(17);
var flatten     = __webpack_require__(33);
var MODE_FLAT   = 'flat';
var MODE_NESTED = 'nested';
var MODES       = [MODE_FLAT, MODE_NESTED];

/**
 * Object wrapping class.
 */
var Homefront = function Homefront(data, mode) {
  this.data = data || {};

  this.setMode(mode);
};

var staticAccessors = { MODE_NESTED: {},MODE_FLAT: {} };

/**
 * Recursively merges given sources into data.
 *
 * @param {...Object} sources One or more, or array of, objects to merge into data (left to right).
 *
 * @return {Homefront}
 */
staticAccessors.MODE_NESTED.get = function () {
  return MODE_NESTED;
};

/**
 * @return {string}
 */
staticAccessors.MODE_FLAT.get = function () {
  return MODE_FLAT;
};

Homefront.prototype.merge = function merge (sources) {
    var this$1 = this;

  sources     = Array.isArray(sources) ? sources : Array.prototype.slice.call(arguments); //eslint-disable-line prefer-rest-params
  var mergeData = [];

  sources.forEach(function (source) {
    if (!source) {
      return;
    }

    if (source instanceof Homefront) {
      source = source.data;
    }

    mergeData.push(this$1.isModeFlat() ? flatten(source) : expand(source));
  });

  extend.apply(extend, [true, this.data].concat(mergeData));

  return this;
};

/**
 * Static version of merge, allowing you to merge objects together.
 *
 * @param {...Object} sources One or more, or array of, objects to merge (left to right).
 *
 * @return {{}}
 */
Homefront.merge = function merge (sources) {
  sources = Array.isArray(sources) ? sources : Array.prototype.slice.call(arguments); //eslint-disable-line prefer-rest-params

  return extend.apply(extend, [true].concat(sources));
};

/**
 * Sets the mode.
 *
 * @param {String} [mode] Defaults to nested.
 *
 * @returns {Homefront} Fluent interface
 *
 * @throws {Error}
 */
Homefront.prototype.setMode = function setMode (mode) {
  mode = mode || MODE_NESTED;

  if (MODES.indexOf(mode) === -1) {
    throw new Error(
      ("Invalid mode supplied. Must be one of \"" + (MODES.join('" or "')) + "\"")
    );
  }

  this.mode = mode;

  return this;
};

/**
 * Gets the mode.
 *
 * @return {String}
 */
Homefront.prototype.getMode = function getMode () {
  return this.mode;
};

/**
 * Expands flat object to nested object.
 *
 * @return {{}}
 */
Homefront.prototype.expand = function expand$1 () {
  return this.isModeNested() ? this.data : expand(this.data);
};

/**
 * Flattens nested object (dot separated keys).
 *
 * @return {{}}
 */
Homefront.prototype.flatten = function flatten$1 () {
  return this.isModeFlat() ? this.data : flatten(this.data);
};

/**
 * Returns whether or not mode is flat.
 *
 * @return {boolean}
 */
Homefront.prototype.isModeFlat = function isModeFlat () {
  return this.mode === MODE_FLAT;
};

/**
 * Returns whether or not mode is nested.
 *
 * @return {boolean}
 */
Homefront.prototype.isModeNested = function isModeNested () {
  return this.mode === MODE_NESTED;
};

/**
 * Method allowing you to set missing keys (backwards-applied defaults) nested.
 *
 * @param {String|Array} key
 * @param {*}          defaults
 *
 * @returns {Homefront}
 */
Homefront.prototype.defaults = function defaults (key, defaults) {
  return this.put(key, Homefront.merge(defaults, this.fetch(key, {})));
};

/**
 * Convenience method. Calls .fetch(), and on null result calls .put() using provided toPut.
 *
 * @param {String|Array} key
 * @param {*}          toPut
 *
 * @return {*}
 */
Homefront.prototype.fetchOrPut = function fetchOrPut (key, toPut) {
  var wanted = this.fetch(key);

  if (wanted === null) {
    wanted = toPut;

    this.put(key, toPut);
  }

  return wanted;
};

/**
 * Fetches value of given key.
 *
 * @param {String|Array} key
 * @param {*}          [defaultValue] Value to return if key was not found
 *
 * @returns {*}
 */
Homefront.prototype.fetch = function fetch (key, defaultValue) {
  defaultValue = typeof defaultValue === 'undefined' ? null : defaultValue;

  if (typeof this.data[key] !== 'undefined') {
    return this.data[key];
  }

  if (this.isModeFlat()) {
    return defaultValue;
  }

  var keys  = Utils.normalizeKey(key);
  var lastKey = keys.pop();
  var tmp   = this.data;

  for (var i = 0; i < keys.length; i += 1) {
    if (typeof tmp[keys[i]] === 'undefined' || tmp[keys[i]] === null) {
      return defaultValue;
    }

    tmp = tmp[keys[i]];
  }

  return typeof tmp[lastKey] === 'undefined' ? defaultValue : tmp[lastKey];
};

/**
 * Sets value for a key (creates object in path when not found).
 *
 * @param {String|Array} key  Array of key parts, or dot separated key.
 * @param {*}          value
 *
 * @returns {Homefront}
 */
Homefront.prototype.put = function put (key, value) {
  if (this.isModeFlat() || key.indexOf('.') === -1) {
      this.data[key] = value;

    return this;
  }

  var keys  = Utils.normalizeKey(key);
  var lastKey = keys.pop();
  var tmp   = this.data;

  keys.forEach(function (val) {
    if (typeof tmp[val] === 'undefined') {
      tmp[val] = {};
    }

    tmp = tmp[val];
  });

  tmp[lastKey] = value;

  return this;
};

/**
 * Removes value by key.
 *
 * @param {String} key
 *
 * @returns {Homefront}
 */
Homefront.prototype.remove = function remove (key) {
  if (this.isModeFlat() || key.indexOf('.') === -1) {
    delete this.data[key];

    return this;
  }

  var normalizedKey = Utils.normalizeKey(key);
  var lastKey     = normalizedKey.pop();
  var source      = this.fetch(normalizedKey);

  if (typeof source === 'object' && source !== null) {
    delete source[lastKey];
  }

  return this;
};

/**
 * Search and return keys and values that match given string.
 *
 * @param {String|Number} phrase
 *
 * @returns {Array}
 */
Homefront.prototype.search = function search (phrase) {
  var found = [];
  var data= this.data;

  if (this.isModeNested()) {
    data = flatten(this.data);
  }

  Object.getOwnPropertyNames(data).forEach(function (key) {
    var searchTarget = Array.isArray(data[key]) ? JSON.stringify(data[key]) : data[key];

    if (searchTarget.search(phrase) > -1) {
      found.push({key: key, value: data[key]});
    }
  });

  return found;
};

Object.defineProperties( Homefront, staticAccessors );

module.exports.flatten   = flatten;
module.exports.expand    = expand;
module.exports.Utils     = Utils;
module.exports.Homefront = Homefront;


/***/ },

/***/ 17:
/***/ function(module, exports) {

"use strict";
'use strict';

var Utils = function Utils () {};

Utils.normalizeKey = function normalizeKey (rest) {
  rest         = Array.isArray(rest) ? rest : Array.prototype.slice.call(arguments);//eslint-disable-line prefer-rest-params
  var key      = rest.shift();
  var normalized = Array.isArray(key) ? Utils.normalizeKey(key) : key.split('.');

  return rest.length === 0 ? normalized : normalized.concat(Utils.normalizeKey(rest));
};

/**
 * Check if `target` is a Plain ol' Javascript Object.
 *
 * @param {*} target
 *
 * @return {boolean}
 */
Utils.isPojo = function isPojo (target) {
  return !(target === null || typeof target !== 'object') && target.constructor === Object;
};

module.exports = Utils;


/***/ },

/***/ 19:
/***/ function(module, exports, __webpack_require__) {

module.exports = (__webpack_require__(0))(1);

/***/ },

/***/ 2:
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.ResolvedViewStrategy = exports.ViewManager = exports.Config = undefined;

var _dec, _class2, _dec2, _class3;

exports.configure = configure;
exports.resolvedView = resolvedView;

var _extend = __webpack_require__(9);

var _extend2 = _interopRequireDefault(_extend);

var _aureliaDependencyInjection = __webpack_require__(3);

var _aureliaTemplating = __webpack_require__(19);

var _aureliaPath = __webpack_require__(34);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }



var Config = exports.Config = function () {
  function Config() {
    

    this.defaults = {
      location: '{{framework}}/{{view}}.html',
      framework: 'bootstrap',
      map: {}
    };
    this.namespaces = {};

    this.namespaces.defaults = this.defaults;
  }

  Config.prototype.configureDefaults = function configureDefaults(configs) {
    (0, _extend2.default)(true, this.defaults, configs);

    return this;
  };

  Config.prototype.configureNamespace = function configureNamespace(name) {
    var _configure;

    var configs = arguments.length <= 1 || arguments[1] === undefined ? { map: {} } : arguments[1];

    var namespace = this.fetch(name);
    (0, _extend2.default)(true, namespace, configs);

    this.configure((_configure = {}, _configure[name] = namespace, _configure));

    return this;
  };

  Config.prototype.configure = function configure(config) {
    (0, _extend2.default)(true, this.namespaces, config);

    return this;
  };

  Config.prototype.fetch = function fetch(properties) {
    if (!this.namespaces[properties]) {
      return this.defaults;
    }

    var result = this.namespaces;
    var args = Array.from(arguments);

    for (var index in args) {
      var key = args[index];
      var value = result[key];
      if (!value) {
        return value;
      }
      result = result[key];
    }

    return result;
  };

  return Config;
}();

function configure(aurelia, configOrConfigure) {
  var config = aurelia.container.get(Config);

  if (typeof configOrConfigure === 'function') {
    return configOrConfigure(config);
  }
  config.configure(configOrConfigure);
}

var ViewManager = exports.ViewManager = (_dec = (0, _aureliaDependencyInjection.inject)(Config), _dec(_class2 = function () {
  function ViewManager(config) {
    

    this.config = config;
  }

  ViewManager.prototype.resolve = function resolve(namespace, view) {
    if (!namespace || !view) {
      throw new Error('Cannot resolve without namespace and view. Got namespace "' + namespace + '" and view "' + view + '" in stead');
    }

    var namespaceOrDefault = Object.create(this.config.fetch(namespace));
    namespaceOrDefault.view = view;

    var location = (namespaceOrDefault.map || {})[view] || namespaceOrDefault.location;

    return render(location, namespaceOrDefault);
  };

  return ViewManager;
}()) || _class2);

function render(template, data) {
  var result = template;

  for (var key in data) {
    var regexString = ['{{', key, '}}'].join('');
    var regex = new RegExp(regexString, 'g');
    var value = data[key];
    result = result.replace(regex, value);
  }

  if (template !== result) {
    result = render(result, data);
  }

  return result;
}

var ResolvedViewStrategy = exports.ResolvedViewStrategy = (_dec2 = (0, _aureliaTemplating.viewStrategy)(), _dec2(_class3 = function () {
  function ResolvedViewStrategy(namespace, view) {
    

    this.namespace = namespace;
    this.view = view;
  }

  ResolvedViewStrategy.prototype.loadViewFactory = function loadViewFactory(viewEngine, compileInstruction, loadContext) {
    var viewManager = viewEngine.container.get(ViewManager);
    var path = viewManager.resolve(this.namespace, this.view);

    compileInstruction.associatedModuleId = this.moduleId;
    return viewEngine.loadViewFactory(this.moduleId ? (0, _aureliaPath.relativeToFile)(path, this.moduleId) : path, compileInstruction, loadContext);
  };

  return ResolvedViewStrategy;
}()) || _class3);
function resolvedView(namespace, view) {
  return (0, _aureliaTemplating.useViewStrategy)(new ResolvedViewStrategy(namespace, view));
}

/***/ },

/***/ 20:
/***/ function(module, exports, __webpack_require__) {

module.exports = (__webpack_require__(0))(6);

/***/ },

/***/ 21:
/***/ function(module, exports) {


module.exports = function equal(arr1, arr2) {
  var length = arr1.length
  if (length !== arr2.length) return false
  for (var i = 0; i < length; i++)
    if (arr1[i] !== arr2[i])
      return false
  return true
}


/***/ },

/***/ 23:
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.Metadata = undefined;

var _aureliaMetadata = __webpack_require__(20);

var _homefront = __webpack_require__(16);



var Metadata = exports.Metadata = function () {
  function Metadata() {
    
  }

  Metadata.forTarget = function forTarget(target) {
    if (typeof target !== 'function') {
      target = target.constructor;
    }

    return _aureliaMetadata.metadata.getOrCreateOwn('spoonx:form:metadata', _homefront.Homefront, target, target.name);
  };

  return Metadata;
}();

/***/ },

/***/ 27:
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.Chart = undefined;

var _aureliaCharts = __webpack_require__(4);



var Chart = exports.Chart = function () {
  function Chart() {
    

    this.settings = {};
    this.dimensions = [];
    this.data = {};
  }

  Chart.prototype.create = function create() {
    warn('create');
  };

  Chart.prototype.update = function update(oldData, newData) {
    warn('update');
  };

  Chart.prototype.destroy = function destroy() {
    warn('destroy');
  };

  return Chart;
}();

function warn(methodName) {
  _aureliaCharts.logger.warn(methodName + ' method not defined for ' + this.library + '\'s type ' + this.type);
}

/***/ },

/***/ 28:
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.chart = chart;

var _config = __webpack_require__(7);

var _aureliaDependencyInjection = __webpack_require__(3);

function chart(namespace, type) {
  var config = _aureliaDependencyInjection.Container.instance.get(_config.Config);

  return function (target) {
    config.registerChart(namespace, type, target);
  };
}

/***/ },

/***/ 29:
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.scales = scales;

var _config = __webpack_require__(7);

var _aureliaDependencyInjection = __webpack_require__(3);

function scales() {
  for (var _len = arguments.length, scaleTypes = Array(_len), _key = 0; _key < _len; _key++) {
    scaleTypes[_key] = arguments[_key];
  }

  var config = _aureliaDependencyInjection.Container.instance.get(_config.Config);

  return function (target) {
    config.registerScales.apply(config, [target].concat(scaleTypes));
  };
}

/***/ },

/***/ 3:
/***/ function(module, exports, __webpack_require__) {

module.exports = (__webpack_require__(0))(3);

/***/ },

/***/ 30:
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.typeScale = typeScale;
exports.typesScales = typesScales;
exports.prop = prop;
exports.unpack = unpack;
exports.unpackProps = unpackProps;
exports.unpackAll = unpackAll;
exports.unpackAllGrouped = unpackAllGrouped;
exports.mapValues = mapValues;
exports.groupBy = groupBy;
exports.reduceByX = reduceByX;
exports.tail = tail;
exports.row = row;
exports.rows = rows;

var _aureliaCharts = __webpack_require__(4);

function typeScale(type) {
  var quantativeTypes = ['date', 'date-time', 'number', 'integer', 'float', 'int'];

  return quantativeTypes.indexOf(type) !== -1 ? _aureliaCharts.quan : _aureliaCharts.qual;
}

function typesScales() {
  for (var _len = arguments.length, types = Array(_len), _key = 0; _key < _len; _key++) {
    types[_key] = arguments[_key];
  }

  return types.map(typeScale);
}

function prop(key) {
  return function value(obj) {
    return obj[key];
  };
}

function unpack(key, data) {
  return data.map(prop(key));
}

function unpackProps(keys, data) {
  return keys.reduce(function (acc, key) {
    acc[key] = unpack(key, data);

    return acc;
  }, {});
}

function unpackAll(data) {
  return unpackProps(Object.keys(data[0]), data);
}

function unpackAllGrouped(groups) {
  return groups.map(function (group) {
    return {
      key: group.key,
      values: unpackAll(group.values)
    };
  });
}

function mapValues(fn, object) {
  return Object.keys(object).reduce(function (acc, key) {
    acc[key] = fn(object[key]);

    return acc;
  }, {});
}

function groupBy(key, objects) {
  var groupIndex = [];
  var index = void 0;

  return objects.reduce(function (acc, obj) {
    var group = obj[key];

    if (groupIndex.indexOf(group) !== -1) {
      index = groupIndex.indexOf(group);
    } else {
      index = groupIndex.push(group) - 1;
      acc[index] = {
        key: group,
        values: []
      };
    }
    acc[index].values.push(obj);

    return acc;
  }, []);
}

function reduceByX(columns) {
  return columns[0].reduce(function (acc, val, index) {
    var vals = row(index, columns);

    acc[val] = acc[val] ? acc[val].concat(vals) : vals;

    return acc;
  }, {});
}

function tail(list) {
  return list.slice(1, Infinity);
}

function row(index, columns) {
  return columns.map(function (column) {
    return column[index];
  });
}

function rows(columns) {
  return columns[0].map(function (field, index) {
    return row(index, columns);
  });
}

/***/ },

/***/ 32:
/***/ function(module, exports) {

"use strict";
'use strict';

/**
 * Expands flat object to nested object.
 *
 * @param {{}} source
 *
 * @return {{}}
 */
module.exports = function expand(source) {
  var destination = {};

  Object.getOwnPropertyNames(source).forEach(function (flatKey) {

    // If the key doesn't contain a dot (isn't nested), just set the value.
    if (flatKey.indexOf('.') === -1) {
      destination[flatKey] = source[flatKey];

      return;
    }

    var tmp  = destination;         // Pointer for the nested object.
    var keys = flatKey.split('.');  // Keys (path) for the nested object.
    var key  = keys.pop();          // The last (deepest) key.

    keys.forEach(function (value) {
      if (typeof tmp[value] === 'undefined') {
        tmp[value] = {};
      }

      tmp = tmp[value];
    });

    tmp[key] = source[flatKey];
  });

  return destination;
};


/***/ },

/***/ 33:
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

var Utils = __webpack_require__(17);

/**
 * Flattens nested object (dot separated keys).
 *
 * @param {{}}      source
 * @param {String}  [basePath]
 * @param {{}}      [target]
 *
 * @return {{}}
 */
module.exports = function flatten(source, basePath, target) {
  basePath = basePath || '';
  target   = target || {};

  Object.getOwnPropertyNames(source).forEach(function (key) {
    if (Utils.isPojo(source[key])) {
      flatten(source[key], basePath + key + '.', target);

      return;
    }

    target[basePath + key] = source[key];
  });

  return target;
};


/***/ },

/***/ 34:
/***/ function(module, exports, __webpack_require__) {

module.exports = (__webpack_require__(0))(7);

/***/ },

/***/ 4:
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.quan = exports.qual = exports.logger = exports.Config = exports.scales = exports.chart = exports.Chart = undefined;

var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };

var _chart = __webpack_require__(27);

Object.defineProperty(exports, 'Chart', {
  enumerable: true,
  get: function get() {
    return _chart.Chart;
  }
});

var _chart2 = __webpack_require__(28);

Object.defineProperty(exports, 'chart', {
  enumerable: true,
  get: function get() {
    return _chart2.chart;
  }
});

var _scales = __webpack_require__(29);

Object.defineProperty(exports, 'scales', {
  enumerable: true,
  get: function get() {
    return _scales.scales;
  }
});

var _utils = __webpack_require__(30);

Object.keys(_utils).forEach(function (key) {
  if (key === "default" || key === "__esModule") return;
  Object.defineProperty(exports, key, {
    enumerable: true,
    get: function get() {
      return _utils[key];
    }
  });
});
exports.configure = configure;

var _config = __webpack_require__(7);

var _aureliaLogging = __webpack_require__(6);

var logger = (0, _aureliaLogging.getLogger)('aurelia-charts');

function configure(aurelia, chartsConfig) {
  aurelia.globalResources('./component/chart-element');

  var config = aurelia.container.get(_config.Config);
  var libraries = Object.keys(config.charts);

  if ((typeof chartsConfig === 'undefined' ? 'undefined' : _typeof(chartsConfig)) === 'object') {
    config.configure(chartsConfig);
  } else if (typeof chartsConfig === 'function') {
    chartsConfig(config);
  } else if (chartsConfig) {
    logger.warn('chart configurations can be a function or an object not a ' + (typeof chartsConfig === 'undefined' ? 'undefined' : _typeof(chartsConfig)) + ' value');
  }

  if (libraries.length === 0) {
    logger.warn('no aurelia-charts plugins installed. Head to the docs and read');
  } else {
    logger.debug('installed ' + libraries.join(' and ') + ' as aurelia-charts libraries');
  }
}

var qual = 'qualitative';
var quan = 'quantitative';

exports.Config = _config.Config;
exports.logger = logger;
exports.qual = qual;
exports.quan = quan;

/***/ },

/***/ 6:
/***/ function(module, exports, __webpack_require__) {

module.exports = (__webpack_require__(0))(5);

/***/ },

/***/ 7:
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.Config = undefined;

var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };

var _arrayEqual = __webpack_require__(21);

var _arrayEqual2 = _interopRequireDefault(_arrayEqual);

var _extend = __webpack_require__(9);

var _extend2 = _interopRequireDefault(_extend);

var _aureliaCharts = __webpack_require__(4);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }



var Config = exports.Config = function () {
  function Config() {
    

    this.defaults = {
      library: undefined,
      type: 'line',
      libraries: {}
    };
    this.charts = {};
    this.scales = [];
  }

  Config.prototype.registerChart = function registerChart(library, type, target) {
    this.charts[library] = this.charts[library] || {};

    this.charts[library][type] = target;

    return this;
  };

  Config.prototype.registerScales = function registerScales(target) {
    for (var _len = arguments.length, scales = Array(_len > 1 ? _len - 1 : 0), _key = 1; _key < _len; _key++) {
      scales[_key - 1] = arguments[_key];
    }

    this.scales.push({
      constructor: target,
      scales: scales
    });

    return this;
  };

  Config.prototype.configure = function configure(defaults) {
    (0, _extend2.default)(true, this, defaults);

    return this;
  };

  Config.prototype.chartsByScale = function chartsByScale() {
    for (var _len2 = arguments.length, scale = Array(_len2), _key2 = 0; _key2 < _len2; _key2++) {
      scale[_key2] = arguments[_key2];
    }

    var result = [];

    this.scales.forEach(function (chartScales) {
      chartScales.scales.forEach(function (chartScale) {
        if ((0, _arrayEqual2.default)(scale, chartScale)) {
          result.push(chartScales.constructor);
        }
      });
    });

    return result;
  };

  Config.prototype.chart = function chart(value) {
    if (typeof value === 'undefined') {
      return this.chart(this.defaults);
    }

    if (typeof value === 'string') {
      return this.chart({
        library: undefined,
        type: value
      });
    }

    if ((typeof value === 'undefined' ? 'undefined' : _typeof(value)) === 'object') {
      var type = value.type || this.defaults.type;
      var libName = value.library || this.defaults.libraries[type] || this.defaults.library;
      var library = this.charts[libName];

      if (typeof library === 'undefined') {
        _aureliaCharts.logger.warn(value.library + ' is not a registered library. Either define a default library or tell what library to use');

        return undefined;
      }

      return library[type];
    }

    return undefined;
  };

  return Config;
}();

/***/ },

/***/ 9:
/***/ function(module, exports) {

"use strict";
'use strict';

var hasOwn = Object.prototype.hasOwnProperty;
var toStr = Object.prototype.toString;

var isArray = function isArray(arr) {
	if (typeof Array.isArray === 'function') {
		return Array.isArray(arr);
	}

	return toStr.call(arr) === '[object Array]';
};

var isPlainObject = function isPlainObject(obj) {
	if (!obj || toStr.call(obj) !== '[object Object]') {
		return false;
	}

	var hasOwnConstructor = hasOwn.call(obj, 'constructor');
	var hasIsPrototypeOf = obj.constructor && obj.constructor.prototype && hasOwn.call(obj.constructor.prototype, 'isPrototypeOf');
	// Not own constructor property must be Object
	if (obj.constructor && !hasOwnConstructor && !hasIsPrototypeOf) {
		return false;
	}

	// Own properties are enumerated firstly, so to speed up,
	// if last one is own, then all properties are own.
	var key;
	for (key in obj) {/**/}

	return typeof key === 'undefined' || hasOwn.call(obj, key);
};

module.exports = function extend() {
	var options, name, src, copy, copyIsArray, clone,
		target = arguments[0],
		i = 1,
		length = arguments.length,
		deep = false;

	// Handle a deep copy situation
	if (typeof target === 'boolean') {
		deep = target;
		target = arguments[1] || {};
		// skip the boolean and the target
		i = 2;
	} else if ((typeof target !== 'object' && typeof target !== 'function') || target == null) {
		target = {};
	}

	for (; i < length; ++i) {
		options = arguments[i];
		// Only deal with non-null/undefined values
		if (options != null) {
			// Extend the base object
			for (name in options) {
				src = target[name];
				copy = options[name];

				// Prevent never-ending loop
				if (target !== copy) {
					// Recurse if we're merging plain objects or arrays
					if (deep && copy && (isPlainObject(copy) || (copyIsArray = isArray(copy)))) {
						if (copyIsArray) {
							copyIsArray = false;
							clone = src && isArray(src) ? src : [];
						} else {
							clone = src && isPlainObject(src) ? src : {};
						}

						// Never move original objects, clone them
						target[name] = extend(deep, clone, copy);

					// Don't bring in undefined values
					} else if (typeof copy !== 'undefined') {
						target[name] = copy;
					}
				}
			}
		}
	}

	// Return the modified object
	return target;
};



/***/ },

/***/ "aurelia-config":
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.Configuration = exports.PluginManager = exports.Config = undefined;

var _dec, _class, _dec2, _class2;

exports.configure = configure;

var _homefront = __webpack_require__(16);

var _aureliaDependencyInjection = __webpack_require__(3);

var _aureliaFramework = __webpack_require__(1);



function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Config = exports.Config = function (_Homefront) {
  _inherits(Config, _Homefront);

  function Config() {
    

    return _possibleConstructorReturn(this, _Homefront.apply(this, arguments));
  }

  return Config;
}(_homefront.Homefront);

var PluginManager = exports.PluginManager = (_dec = (0, _aureliaDependencyInjection.inject)(Config), _dec(_class = function () {
  function PluginManager(config) {
    

    this.config = config;
  }

  PluginManager.prototype.normalized = function normalized(plugins, handler) {
    plugins.forEach(function (pluginDefinition) {
      pluginDefinition = pluginDefinition || {};

      if (typeof pluginDefinition === 'string') {
        pluginDefinition = { moduleId: pluginDefinition };
      }

      if (typeof pluginDefinition.config === 'undefined') {
        pluginDefinition.config = {};
      }

      handler(pluginDefinition);
    });
  };

  PluginManager.prototype.configure = function configure(use, plugins) {
    for (var _len = arguments.length, appConfigs = Array(_len > 2 ? _len - 2 : 0), _key = 2; _key < _len; _key++) {
      appConfigs[_key - 2] = arguments[_key];
    }

    var _this2 = this;

    var loadConfigs = [];
    var pluginConfigs = [];

    var loadConfig = function loadConfig(plugin) {
      return use.aurelia.loader.loadModule(plugin.moduleId).then(function (module) {
        return Config.merge(plugin.config, module.config);
      });
    };

    this.normalized(plugins, function (plugin) {
      loadConfigs.push(loadConfig(plugin));

      pluginConfigs.push(plugin.config);

      _this2.config.fetchOrPut(plugin.moduleId, {});

      use.plugin(plugin.moduleId, plugin.rootConfig ? _this2.config.data : _this2.config.data[plugin.moduleId]);
    });

    return Promise.all(loadConfigs).then(function () {
      return _this2.config.merge(pluginConfigs.concat(appConfigs));
    });
  };

  return PluginManager;
}()) || _class);
var Configuration = exports.Configuration = (_dec2 = (0, _aureliaDependencyInjection.resolver)(), _dec2(_class2 = function () {
  function Configuration(namespace) {
    

    this._namespace = namespace;
  }

  Configuration.prototype.get = function get(container) {
    return container.get(Config).fetch(this._namespace);
  };

  Configuration.of = function of(namespace) {
    return new Configuration(namespace);
  };

  return Configuration;
}()) || _class2);
function configure(use, callback) {
  var pluginManager = use.container.get(PluginManager);

  return callback(function (plugins) {
    for (var _len2 = arguments.length, appConfigs = Array(_len2 > 1 ? _len2 - 1 : 0), _key2 = 1; _key2 < _len2; _key2++) {
      appConfigs[_key2 - 1] = arguments[_key2];
    }

    return pluginManager.configure.apply(pluginManager, [use, plugins].concat(appConfigs));
  });
}

/***/ },

/***/ "aurelia-form":
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.config = exports.logger = undefined;

var _index = __webpack_require__("aurelia-form/decorator/index.js");

Object.keys(_index).forEach(function (key) {
  if (key === "default" || key === "__esModule") return;
  Object.defineProperty(exports, key, {
    enumerable: true,
    get: function get() {
      return _index[key];
    }
  });
});
exports.configure = configure;

var _aureliaViewManager = __webpack_require__(2);

var _aureliaLogging = __webpack_require__(6);

var logger = exports.logger = (0, _aureliaLogging.getLogger)('aurelia-form');

function configure(aurelia, config) {
  aurelia.aurelia.use.plugin('aurelia-view-manager');

  aurelia.container.get(_aureliaViewManager.Config).configureNamespace('spoonx/form', {
    location: './view/{{framework}}/{{view}}.html'
  });

  var defaultComponents = ['aurelia-form', 'form-element', 'form-label', 'form-button', 'form-help', 'form-error', 'form-group', 'entity-form'];

  var defaultElements = ['input', 'checkbox', 'radio', 'select', 'textarea', 'association'];

  aurelia.globalResources.apply(aurelia, ['./attribute/prefixed'].concat(defaultComponents.map(function (component) {
    return './component/' + component;
  }), defaultElements.map(function (component) {
    return './component/form-' + component;
  })));

  config.elements = config.elements || {};

  defaultElements.forEach(function (element) {
    config.elements[element] = config.elements[element] || 'form-' + element;
  });
}

var config = exports.config = {
  'aurelia-form': {
    defaultElement: 'input',
    defaultBehavior: 'regular',
    defaultLabelClasses: '',
    defaultElementClasses: '',
    elements: {},
    validation: {},

    submitButton: {
      enabled: true,
      options: ['primary'],
      label: 'Submit'
    },

    aliases: {
      enum: 'radio',
      int: 'input',
      integer: 'input',
      number: 'input',
      float: 'input',
      string: 'input',
      bool: 'checkbox',
      boolean: 'checkbox',
      text: 'textarea'
    }
  }
};

/***/ },

/***/ "aurelia-form/decorator/autofocus.js":
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.autofocus = autofocus;

var _field = __webpack_require__("aurelia-form/decorator/field.js");

function autofocus() {
  var value = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : true;

  return (0, _field.field)(value, 'autofocus');
}

/***/ },

/***/ "aurelia-form/decorator/disabled.js":
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.disabled = disabled;

var _field = __webpack_require__("aurelia-form/decorator/field.js");

function disabled() {
  var value = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : true;

  return (0, _field.field)(value, 'disabled');
}

/***/ },

/***/ "aurelia-form/decorator/element.js":
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.element = element;

var _field = __webpack_require__("aurelia-form/decorator/field.js");

function element(value) {
  return (0, _field.field)(value, 'element');
}

/***/ },

/***/ "aurelia-form/decorator/field.js":
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.field = field;

var _metadata = __webpack_require__(23);

function field(value, option) {
  return function (target, property) {
    _metadata.Metadata.forTarget(target.constructor).put('fields.' + property + '.' + option, value);
  };
}

/***/ },

/***/ "aurelia-form/decorator/index.js":
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});

var _placeholder = __webpack_require__("aurelia-form/decorator/placeholder.js");

Object.defineProperty(exports, 'placeholder', {
  enumerable: true,
  get: function get() {
    return _placeholder.placeholder;
  }
});

var _label = __webpack_require__("aurelia-form/decorator/label.js");

Object.defineProperty(exports, 'label', {
  enumerable: true,
  get: function get() {
    return _label.label;
  }
});

var _element = __webpack_require__("aurelia-form/decorator/element.js");

Object.defineProperty(exports, 'element', {
  enumerable: true,
  get: function get() {
    return _element.element;
  }
});

var _position = __webpack_require__("aurelia-form/decorator/position.js");

Object.defineProperty(exports, 'position', {
  enumerable: true,
  get: function get() {
    return _position.position;
  }
});

var _autofocus = __webpack_require__("aurelia-form/decorator/autofocus.js");

Object.defineProperty(exports, 'autofocus', {
  enumerable: true,
  get: function get() {
    return _autofocus.autofocus;
  }
});

var _disabled = __webpack_require__("aurelia-form/decorator/disabled.js");

Object.defineProperty(exports, 'disabled', {
  enumerable: true,
  get: function get() {
    return _disabled.disabled;
  }
});

var _readonly = __webpack_require__("aurelia-form/decorator/readonly.js");

Object.defineProperty(exports, 'readonly', {
  enumerable: true,
  get: function get() {
    return _readonly.readonly;
  }
});

var _required = __webpack_require__("aurelia-form/decorator/required.js");

Object.defineProperty(exports, 'required', {
  enumerable: true,
  get: function get() {
    return _required.required;
  }
});

var _noRender = __webpack_require__("aurelia-form/decorator/noRender.js");

Object.defineProperty(exports, 'noRender', {
  enumerable: true,
  get: function get() {
    return _noRender.noRender;
  }
});

var _inputType = __webpack_require__("aurelia-form/decorator/inputType.js");

Object.defineProperty(exports, 'inputType', {
  enumerable: true,
  get: function get() {
    return _inputType.inputType;
  }
});

var _options = __webpack_require__("aurelia-form/decorator/options.js");

Object.defineProperty(exports, 'options', {
  enumerable: true,
  get: function get() {
    return _options.options;
  }
});

/***/ },

/***/ "aurelia-form/decorator/inputType.js":
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.inputType = inputType;

var _field = __webpack_require__("aurelia-form/decorator/field.js");

function inputType(value) {
  return (0, _field.field)(value, 'type');
}

/***/ },

/***/ "aurelia-form/decorator/label.js":
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.label = label;

var _field = __webpack_require__("aurelia-form/decorator/field.js");

function label(value) {
  return (0, _field.field)(value, 'label');
}

/***/ },

/***/ "aurelia-form/decorator/noRender.js":
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.noRender = noRender;

var _field = __webpack_require__("aurelia-form/decorator/field.js");

function noRender() {
  var value = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : true;

  return (0, _field.field)(value, 'noRender');
}

/***/ },

/***/ "aurelia-form/decorator/options.js":
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.options = options;

var _field = __webpack_require__("aurelia-form/decorator/field.js");

function options() {
  var value = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};

  return (0, _field.field)(value, 'options');
}

/***/ },

/***/ "aurelia-form/decorator/placeholder.js":
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.placeholder = placeholder;

var _field = __webpack_require__("aurelia-form/decorator/field.js");

function placeholder(value) {
  return (0, _field.field)(value, 'placeholder');
}

/***/ },

/***/ "aurelia-form/decorator/position.js":
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.position = position;

var _field = __webpack_require__("aurelia-form/decorator/field.js");

function position() {
  var value = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : -1;

  return (0, _field.field)(value, 'position');
}

/***/ },

/***/ "aurelia-form/decorator/readonly.js":
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.readonly = readonly;

var _field = __webpack_require__("aurelia-form/decorator/field.js");

function readonly() {
  var value = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : true;

  return (0, _field.field)(value, 'readonly');
}

/***/ },

/***/ "aurelia-form/decorator/required.js":
/***/ function(module, exports, __webpack_require__) {

"use strict";
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.required = required;

var _field = __webpack_require__("aurelia-form/decorator/field.js");

function required() {
  var value = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : true;

  return (0, _field.field)(value, 'required');
}

/***/ }

/******/ });
//# sourceMappingURL=aurelia.js.map