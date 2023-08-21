import React, {useState, useEffect} from 'react';
import {Container, Grid, Typography, Button, Box} from '@mui/material';
import TicketForm from '../src/components/TicketForm';
import TicketTable from '../src/components/TicketTable';
import {fetchTickets, buyTicket, fetchTicketsByDate} from '../api';

function Tickets() {
    // set tickets
    const [tickets, setTickets] = useState([]);
    // set date
    const [selectedDate, setSelectedDate] = useState(new Date().toISOString().slice(0, 16));
    // set ticket type
    const [selectedTicketType, setSelectedTicketType] = useState('');
    // set ticket prices
    const ticketPrices = {
        '1': 5.0,
        '2': 10.0,
        '3': 7,
    };
    
    // fetch tickets
    useEffect(() => {
        fetchTickets().then((data) => setTickets(data));
    }, []);

    // handle date change
    const handleDateChange = (event) => {
        setSelectedDate(event.target.value);
    };

    // handle ticket type change
    const handleTicketTypeChange = (event) => {
        setSelectedTicketType(event.target.value);
    };

    // handle submit
    const handleSubmit = (event) => {
        event.preventDefault();
        buyTicket(selectedTicketType, ticketPrices, selectedDate).then((success) => {
            if (success) {
                fetchTickets().then((data) => setTickets(data));
            }
        });
    };

    // calculate total tickets and total price
    const totalTickets = tickets.length;
    const totalPrice = tickets.reduce((sum, ticket) => sum + ticket.preis, 0);

    return (
        <Container>
            <Box sx={{ mt: 4, mb: 4, textAlign: 'center' }}>
                <Typography variant="h2" gutterBottom>
                    Tickets
                </Typography>
            </Box>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <TicketForm
                        selectedDate={selectedDate}
                        handleDateChange={handleDateChange}
                        selectedTicketType={selectedTicketType}
                        handleTicketTypeChange={handleTicketTypeChange}
                        handleSubmit={handleSubmit}
                    />
                </Grid>
                <Grid item xs={5}>
                    <Button
                        variant="contained"
                        color="secondary"
                        onClick={() => fetchTicketsByDate(selectedDate).then((data) => setTickets(data))}
                    >
                        Tickets nach Datum abrufen
                    </Button>
                </Grid>
                <Grid item xs={12}>
                    <TicketTable tickets={tickets} />
                </Grid>
            </Grid>
            <Box sx={{ mt: 4, textAlign: 'center' }}>
                <Typography variant="h6" gutterBottom>
                    Gesamtanzahl der Tickets: {totalTickets}
                </Typography>
                <Typography variant="h6">
                    Gesamtsumme: {totalPrice.toFixed(2)} €
                </Typography>
            </Box>
            <Button variant="contained"
                    display="flex"
                    justifyContent="center"
                    href="/"
                    onClick={async () => {
                        await fetch('http://localhost:5173/');
                    }}>Back</Button>
        </Container>
    );
}

export default Tickets;