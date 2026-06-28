import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { Species, CreateSpecies, UpdateSpecies } from '../models/species';

@Injectable({
  providedIn: 'root'
})
export class SpeciesService {

  private apiUrl = environment.apiUrl + '/species';

  constructor(private http: HttpClient) { }

  getAll(): Observable<Species[]> {
    return this.http.get<Species[]>(this.apiUrl);
  }

  create(species: CreateSpecies): Observable<Species> {
    return this.http.post<Species>(this.apiUrl, species);
  }

  update(id: number, species: UpdateSpecies): Observable<Species> {
    return this.http.put<Species>(this.apiUrl + '/' + id, species);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(this.apiUrl + '/' + id);
  }
}