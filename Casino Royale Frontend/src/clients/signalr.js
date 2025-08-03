import * as signalR from '@microsoft/signalr';

const connection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:7202/casino-hub') 
    .withAutomaticReconnect()
    .build();

export default connection;