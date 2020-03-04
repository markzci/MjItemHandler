const uri = 'api/Items';

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function getItemsNameMaxPrice() {
    fetch(uri + '/MaxPrice')
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function getItemsByName(input) {
    fetch(uri + '/' + input)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}


function _displayItems(data) {
    const tBody = document.getElementById('items');
    tBody.innerHTML = '';

    let itemCount = data.length;

    if (itemCount > 0) {
        data.forEach(item => {
            let tr = tBody.insertRow();
            let td1 = tr.insertCell(0);
            let td2 = tr.insertCell(1);
            let textNode = document.createTextNode(item.name);
            let priceNode = document.createTextNode("$" + item.cost);

            td1.appendChild(textNode);
            document.createElement("br")
            td2.appendChild(priceNode);

        });
    } else {
        let tr = tBody.insertRow();
        let td1 = tr.insertCell(0);
        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(data.name);
        let priceNode = document.createTextNode("$" + data.cost);

        td1.appendChild(textNode);
        document.createElement("br")
        td2.appendChild(priceNode);
    }

    items = data;
}

//click handlers

document.getElementById('list-by-name-price').addEventListener('click', function (e) {
    e.preventDefault();
    getItemsNameMaxPrice();
});

document.getElementById('search-by-name').addEventListener('click', function (e) {
    e.preventDefault
    let input = document.getElementById('input-item-name').value;
    console.log(document.getElementById('input-item-name').value);
    getItemsByName(input);
});
