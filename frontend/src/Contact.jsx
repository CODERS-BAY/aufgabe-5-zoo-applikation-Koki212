import * as React from 'react';
import Button from '@mui/material/Button';
import {Container} from "@mui/material";

// site for contact information
export default function Contact() {
    return (
        <Container>
            <h1>
                Hi visitor!
            </h1>
            <p>This site is only a prototype and only for testing purposes.
                So there is no contact information available.
            </p>
            <p>
                The developer of this site is <u>Kostadin Rizov</u>.
                You can find him on GitHub: <strong>Koki212 </strong>
                or just talk to him in the CodersBay.
            </p>
            <p>Thank you for visiting this site!</p>
            <Button className="backbuttons"
                    variant="contained"
                    justifyContent="center"
                    alignItems="center"
                    href="/"
                    onClick={async () => {
                        await fetch('http://localhost:5173/');
                    }}>Back</Button>
        </Container>
    );
}