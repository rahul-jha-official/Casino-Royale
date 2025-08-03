import api from './api';

export default{
    async signup(data) {
        try {
            const request = {
                "email": data.email,
                "name": data.name,
                "password": data.password
            };

            const response = await api.post('auth/register', request);
            return response;
        }
        catch (error) {
            throw error; // Re-throw the error for further handling
        }
    },

    async login(data) {
        try {
            const request = {
                "email": data.email,
                "password": data.password
            };

            const response = await api.post('auth/login', request);
            const {token, email} = response.data;
            const payload = JSON.parse(atob(token.split('.')[1]));
            return {
                token,
                user: {
                    email,
                    role: payload.role,
                    name: payload.fullname,
                    id: payload.id
                }
            };
        }
        catch (error) {
            throw error; // Re-throw the error for further handling
        }
    }
}

// import api from './api';
// export default {
//     async testSimple() {
//         try {
//             const response = await api.get('/test/second');
//             console.log('Test successful:', response);
//         } catch (error) {
//             console.error('Test failed:', error);
//             throw error; // Re-throw the error for further handling
//         }
//     }
// }