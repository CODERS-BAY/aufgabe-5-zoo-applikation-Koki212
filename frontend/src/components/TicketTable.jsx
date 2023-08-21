import React from 'react';
import { Table, TableHead, TableRow, TableCell, TableBody, Paper } from '@mui/material';

function TicketTable({ tickets }) {
    return (
        <Paper>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell>ID</TableCell>
                        <TableCell>Typ</TableCell>
                        <TableCell>Preis</TableCell>
                        <TableCell>Verkaufsdatum</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {tickets.map((ticket) => (
                        <TableRow key={ticket.id}>
                            <TableCell>{ticket.id}</TableCell>
                            <TableCell>{['Kinder', 'Erwachsener', 'Senioren'][ticket.type]}</TableCell>
                            <TableCell>{ticket.preis}</TableCell>
                            <TableCell>{ticket.verkaufsdatum}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </Paper>
    );
}

export default TicketTable;