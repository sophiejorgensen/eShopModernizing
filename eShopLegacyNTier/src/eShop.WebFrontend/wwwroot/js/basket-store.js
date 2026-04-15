// Shared basket store using localStorage — persists across page navigations.
// Used by both index.html (catalog) and basket.html.
var BasketStore = (function() {
    var KEY = 'eshop_basket';

    function load() {
        try { return JSON.parse(localStorage.getItem(KEY)) || []; }
        catch(e) { return []; }
    }

    function save(items) {
        localStorage.setItem(KEY, JSON.stringify(items));
    }

    return {
        getAll: function() { return load(); },
        getItems: function() { return load(); },

        add: function(product) {
            var items = load();
            var found = false;
            for (var i = 0; i < items.length; i++) {
                if (items[i].id === product.id) {
                    items[i].quantity++;
                    found = true;
                    break;
                }
            }
            if (!found) {
                items.push({
                    id: product.id,
                    name: product.name,
                    unitPrice: product.unitPrice,
                    quantity: 1,
                    picture: product.picture
                });
            }
            save(items);
        },

        updateQuantity: function(index, qty) {
            var items = load();
            if (index >= 0 && index < items.length) {
                items[index].quantity = Math.max(0, qty);
                if (items[index].quantity === 0) items.splice(index, 1);
                save(items);
            }
        },

        remove: function(index) {
            var items = load();
            items.splice(index, 1);
            save(items);
        },

        clear: function() { save([]); },

        totalItems: function() {
            var items = load();
            var count = 0;
            for (var i = 0; i < items.length; i++) count += items[i].quantity;
            return count;
        },

        totalPrice: function() {
            var items = load();
            var sum = 0;
            for (var i = 0; i < items.length; i++) sum += items[i].unitPrice * items[i].quantity;
            return Math.round(sum * 100) / 100;
        }
    };
})();
