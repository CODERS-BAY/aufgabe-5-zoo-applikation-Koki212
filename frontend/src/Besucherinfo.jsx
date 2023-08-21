import * as React from 'react';
import Button from '@mui/material/Button';
import Popover from '@mui/material/Popover';
import Typography from '@mui/material/Typography';
import {Container} from "@mui/material";
import {Box} from "@mui/system";

// data for animals
const animalData = [
    {name: 'Löwe', habitat: ' 1', imageUrl: 'src/assets/lion-3583963_1280.jpg'},
    {name: 'Fisch', habitat: 'Aquarium', imageUrl: 'src/assets/fish-3322230_1280.jpg'},
    {name: 'Adler', habitat: ' 2', imageUrl: 'src/assets/eagle-2247269_1280.jpg'},
    {name: 'Pinguin', habitat: ' 3', imageUrl: 'src/assets/birds-7108368_1280.jpg'},
    {name: 'Giraffe', habitat: ' 4', imageUrl: 'src/assets/animal-171318_1280.jpg'},
];

export default function Besucherinfo() {
    const [anchorEl, setAnchorEl] = React.useState(null);
    const [selectedAnimal, setSelectedAnimal] = React.useState(null);
    
    // for hovering over animal
    const handleMouseOver = (event, animal) => {
        setAnchorEl(event.currentTarget);
        setSelectedAnimal(animal);
    };

    // for clicking outside of animal
    const handleClose = () => {
        setAnchorEl(null);
        setSelectedAnimal(null);
    };

    const open = Boolean(anchorEl);
    const id = open ? 'simple-popover' : undefined;

    return (
        <Container>
            <Box sx={{ mt: 4, mb: 4, textAlign: 'left' }}>
                <Typography variant="h2" textAlign={"center"} gutterBottom>
                    Informationen
                </Typography>
            </Box>
            <div style={{display: 'flex', justifyContent: 'center', gap: '20px', marginTop: '20px'}}>
                {animalData.map(animal => (
                    <div
                        key={animal.name}
                        style={{
                            backgroundImage: `url(${animal.imageUrl})`,
                            backgroundSize: 'cover',
                            backgroundPosition: 'center',
                            width: '120px',
                            height: '120px',
                            borderRadius: '50%',
                            cursor: 'pointer',
                        }}
                        onMouseOver={(event) => handleMouseOver(event, animal)}
                    />
                ))}
            </div>
            <Popover
                id={id}
                open={open}
                anchorEl={anchorEl}
                onClose={handleClose}
                anchorOrigin={{
                    vertical: 'bottom',
                    horizontal: 'center',
                }}
                transformOrigin={{
                    vertical: 'top',
                    horizontal: 'center',
                }}
            >
                <Typography sx={{p: 2}}>
                    {selectedAnimal && (
                        <div>
                            <strong>{selectedAnimal.name}</strong><br/>
                            Gehege-Nr: {selectedAnimal.habitat}
                        </div>
                    )}
                </Typography>
            </Popover>
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