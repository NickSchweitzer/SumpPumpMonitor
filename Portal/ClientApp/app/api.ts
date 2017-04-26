import { HttpClient } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';
import { DataPoint, SumpPump } from './models';

@inject(HttpClient)
export class Api {
    private httpClient: HttpClient;
    private apiBase: string;

    constructor(http: HttpClient) {
        this.httpClient = http;
        this.apiBase = '/api/'
    }

    getDataPoints(pumpId: string): Promise<DataPoint[]> {

        return this.httpClient.fetch(this.apiBase + 'pumps/data/' + pumpId)
            .then(result => result.json() as Promise<DataPoint[]>)
            .then(data => {
                return data;
            });
    }

    getPumps(): Promise<SumpPump[]> {

        return this.httpClient.fetch(this.apiBase + 'pumps/')
            .then(result => result.json() as Promise<SumpPump[]>)
            .then(data => {
                return data;
            });
    }

    getPump(pumpId: string): Promise<SumpPump> {

        return this.httpClient.fetch(this.apiBase + "pumps/" + pumpId)
            .then(result => result.json() as Promise<SumpPump>)
            .then(data => {
                return data;
            });
    }

}