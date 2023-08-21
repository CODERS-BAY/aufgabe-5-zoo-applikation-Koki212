// Server URL
const API_BASE_URL = 'http://localhost:5207/api';

// fetching tickets from server
export const fetchTickets = async () => {
    try {
        const response = await fetch(`${API_BASE_URL}/Kassierer/tickets`);
        if (!response.ok) throw new Error(response.statusText);
        return await response.json();
    } catch (error) {
        console.error('Fehler beim Abrufen der Tickets:', error);
        return [];
    }
};

// buying tickets and sending them to server with POST
export const buyTicket = async (ticketType, ticketPrices, selectedDate) => {
    try {
        const response = await fetch(`${API_BASE_URL}/Kassierer/buy`, {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({
                type: parseInt(ticketType, 10),
                preis: ticketPrices[ticketType],
                verkaufsdatum: selectedDate,
            }),
        });
        if (!response.ok) throw new Error(response.statusText);
        return true;
    } catch (error) {
        console.error('Fehler beim Kauf des Tickets:', error);
        return false;
    }
};

// fetching tickets by date from server
export const fetchTicketsByDate = async (date) => {
    try {
        const response = await fetch(`${API_BASE_URL}/Kassierer/tickets/date/${date}`);
        if (!response.ok) throw new Error(response.statusText);
        const data = await response.json();
        return data.tickets;
    } catch (error) {
        console.error('Fehler beim Abrufen der Tickets nach Datum:', error);
        return [];
    }
};

// fetching animals assigned to a employee from server
export const fetchAssignedAnimals = async (tierpflegerId) => {
    const response = await fetch(`${API_BASE_URL}/Tierpfleger/${tierpflegerId}/tiere`);
    if (!response.ok) throw new Error(`API request failed with status ${response.status}`);
    return await response.json();
};

// updating animal data on server
export const updateAnimal = async (animalId, updatedData) => {
    const response = await fetch(`${API_BASE_URL}/Tierpfleger/tiere/${animalId}/${updatedData.gehegeId}`, {
        method: 'PUT',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(updatedData),
    });
    if (!response.ok) throw new Error(`API request failed with status ${response.status}`);
    const responseBody = await response.text();
    if (responseBody) {
        return JSON.parse(responseBody);
    } else {
        throw new Error('Error updating animal: Empty response');
    }
};