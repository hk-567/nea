function openNavmenu() {
    document.getElementById("mySidenavmenu").style.width = "260px"; // navigation bar functionality 
}
function closeNav() {
    document.getElementById("mySidenavmenu").style.width = "0";
}
const addServiceBtn = document.querySelector('#add-service-btn');
const servicesContainer = document.querySelector('#services');

addServiceBtn.addEventListener('click', () => {
    const serviceTemplate = document.querySelector('.service');
    const newService = serviceTemplate.cloneNode(true);
    servicesContainer.appendChild(newService);
});
function showSuccessMessage(message) {
    var alertContainer = document.createElement('div');
    alertContainer.classList.add('alert', 'alert-success');
    alertContainer.setAttribute('role', 'alert');

    var closeButton = document.createElement('button');
    closeButton.classList.add('close');
    closeButton.setAttribute('type', 'button');
    closeButton.setAttribute('data-dismiss', 'alert');
    closeButton.setAttribute('aria-label', 'Close');

    var closeIcon = document.createElement('span');
    closeIcon.setAttribute('aria-hidden', 'true');
    closeIcon.innerHTML = '&times;';

    closeButton.appendChild(closeIcon);
    alertContainer.appendChild(closeButton);

    var messageText = document.createTextNode(message);
    alertContainer.appendChild(messageText);

    var alertWrapper = document.getElementById('alert-wrapper');
    alertWrapper.appendChild(alertContainer);
}
