import React, {useState, useCallback} from 'react';
import {
    Container,
    Grid,
    Card,
    CardMedia,
    CardContent,
    Typography,
    TextField,
    CircularProgress,
    Button,
    IconButton,
} from '@mui/material';
import {Box} from '@mui/system';
import SearchIcon from '@mui/icons-material/Search';
import {fetchAssignedAnimals, updateAnimal, fetchAssignedEmployees} from '../api.js';

function Mitarbeiter() {
    const [tierpflegerId, setTierpflegerId] = useState('');
    const [assignedAnimals, setAssignedAnimals] = useState([]);
    const [loading, setLoading] = useState(false);

    const debounce = (func, wait) => {
        let timeout;
        return (...args) => {
            clearTimeout(timeout);
            timeout = setTimeout(() => func.apply(this, args), wait);
        };
    };

    const fetchAssignedEmployee = useCallback(
        debounce(async (id) => {
            setLoading(true);
            try {
                const data = await fetchAssignedEmployees(tierpflegerId);
                setTierpflegerId(data);
            } catch (error) {
                console.error('Error fetching assigned employee:', error);
            }
            setLoading(false);
        }, 500),
        []
    );
    
    const fetchAssignedAnimalsDebounced = useCallback(
        debounce(async (id) => {
            setLoading(true);
            try {
                const data = await fetchAssignedAnimals(id);
                setAssignedAnimals(data);
            } catch (error) {
                console.error('Error fetching assigned animals:', error);
            }
            setLoading(false);
        }, 500),
        []
    );

    const handleUpdateAnimal = async (animalId) => {
        const updatedData = {
            gattung: document.getElementById(`gattung-${animalId}`).value,
            nahrung: document.getElementById(`nahrung-${animalId}`).value,
            gehegeId: parseInt(document.getElementById(`gehegeId-${animalId}`).value),
        };
        try {
            await updateAnimal(animalId, updatedData);
            setAssignedAnimals((prevAnimals) =>
                prevAnimals.map((animal) => (animal.id === animalId ? {...animal, ...updatedData} : animal))
            );
        } catch (error) {
            console.error('Error updating animal:', error);
        }
    };

    return (
        <Container>
            <Box sx={{mt: 4, mb: 4, textAlign: 'left'}}>
                <Typography variant="h2" gutterBottom>
                    Mitarbeiter
                </Typography>
            </Box>
            <Box sx={{mb: 4, display: 'flex', alignItems: 'center'}}>
                <TextField
                    label="Tierpfleger-ID"
                    variant="outlined"
                    value={tierpflegerId}
                    onChange={(e) => setTierpflegerId(e.target.value)}
                    onKeyPress={(e) => {
                        if (e.key === 'Enter') {
                            fetchAssignedEmployee(tierpflegerId);
                            fetchAssignedAnimalsDebounced(tierpflegerId);
                            e.preventDefault();
                        }
                    }}
                    width="50%"
                    sx={{mr: 1}}
                />
                <IconButton
                    onClick={() => fetchAssignedAnimalsDebounced(tierpflegerId) && fetchAssignedEmployee(tierpflegerId)}
                    color="black"
                >
                    <SearchIcon/>
                </IconButton>
            </Box>
            {loading ? (
                <Box sx={{display: 'flex', justifyContent: 'center', mt: 4}}>
                    <CircularProgress/>
                </Box>
            ) : (
                <Grid container spacing={5}>
                    {assignedAnimals.map((animal) => (
                        <Grid item xs={1} sm={1} md={3} key={animal.id}>
                            <Card>
                                <CardMedia
                                    alt={animal.gattung}
                                    height="2"
                                />
                                <CardContent>
                                    <Typography variant="h6" gutterBottom>
                                        {animal.gattung}
                                    </Typography>
                                    <Typography variant="body2" color="textSecondary">
                                        Nahrung: {animal.nahrung}
                                    </Typography>
                                    <Typography variant="body2" color="textSecondary">
                                        Gehege ID: {animal.gehegeId}
                                    </Typography>
                                    <Button
                                        variant="contained"
                                        color="primary"
                                        sx={{mt: 2, width: '100%'}}
                                        onClick={() => handleUpdateAnimal(animal.id)}
                                    >
                                        Aktualisieren
                                    </Button>
                                </CardContent>
                            </Card>
                        </Grid>
                    ))}
                </Grid>
            )
            }
            <Button variant="contained"
                    href="/" sx={{mt: 4}}
                    onClick={async () => {
                        await fetch('http://localhost:5173/');
                    }}>Back</Button>
        </Container>
    )
        ;
}

export default Mitarbeiter;
