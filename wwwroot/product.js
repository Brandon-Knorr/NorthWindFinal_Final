document.addEventListener("DOMContentLoaded", function() {
  fetchProducts();
});
document.getElementById("CategoryId").addEventListener("change", (e) => {
  document.getElementById('product_rows').dataset['id'] = e.target.value;
  fetchProducts();
});
document.getElementById('Discontinued').addEventListener("change", (e) => {
  fetchProducts();
});
// delegated event listener
document.getElementById('product_rows').addEventListener("click", (e) => {
  p = e.target.parentElement;
  if (p.classList.contains('product')) {
    e.preventDefault()
    // console.log(p.dataset['id']);
    document.getElementById('ProductId').innerHTML = p.dataset['id'];
    document.getElementById('ProductName').innerHTML = p.dataset['name'];
    document.getElementById('UnitPrice').innerHTML = Number(p.dataset['price']).toFixed(2);
    display_total();
    const cart = new bootstrap.Modal('#cartModal', {}).show();
  }
});
const display_total = () => {
  const total = parseInt(document.getElementById('Quantity').value) * Number(document.getElementById('UnitPrice').innerHTML);
  document.getElementById('Total').innerHTML = numberWithCommas(total.toFixed(2));
}
// update total when cart quantity is changed
document.getElementById('Quantity').addEventListener("change", (e) => {
  display_total();
});
// function to display commas in number
const numberWithCommas = x => x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
async function fetchProducts() {
  const id = document.getElementById('product_rows').dataset['id'];
  const discontinued = document.getElementById('Discontinued').checked ? "" : "/discontinued/false";
  const { data: fetchedProducts } = await axios.get(`../../api/category/${id}/product${discontinued}`);
  // console.log(fetchedProducts);
  let product_rows = "";
  fetchedProducts.map(product => {
    const css = product.discontinued ? " discontinued" : "";
    product_rows += 
      `<tr class="product${css}" data-id="${product.productId}" data-name="${product.productName}" data-price="${product.unitPrice}">
        <td>${product.productName}</td>
        <td class="text-end">${product.unitPrice.toFixed(2)}</td>
        <td class="text-end">${product.unitsInStock}</td>
      </tr>`;
  });
  document.getElementById('product_rows').innerHTML = product_rows;
}
