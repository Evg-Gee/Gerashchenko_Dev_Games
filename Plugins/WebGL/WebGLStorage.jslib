mergeInto(LibraryManager.library, {
    WebGL_SaveToStorage: function (key, data) {
        try {
            var keyStr = UTF8ToString(key);
            var dataStr = UTF8ToString(data);
            localStorage.setItem(keyStr, dataStr);
            return 1;
        } catch (e) {
            console.error("Save error:", e);
            return 0;
        }
    },

    WebGL_LoadFromStorage: function (key) {
        try {
            var keyStr = UTF8ToString(key);
            var data = localStorage.getItem(keyStr);
            return data ? UTF8ToString(data) : "";
        } catch (e) {
            console.error("Load error:", e);
            return "";
        }
    },
    Free: function(ptr) {
            if (ptr) _free(ptr);
        }
});