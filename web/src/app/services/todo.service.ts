import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Todo } from '../models/todo';
import { Observable } from 'rxjs';

@Injectable({providedIn: 'root'})
export class TodoService {
    private readonly http = inject(HttpClient);
    private readonly url = "/api/todos";

    getAll(completed?: boolean): Observable<Todo[]> {
        return this.http.get<Todo[]>(this.url, {
            params: completed === undefined ? {} : { completed }
        });
    }

    add(title: string): Observable<Todo> {
        return this.http.post<Todo>(this.url, { title });
    }

    remove(id: string): Observable<void> {
        return this.http.delete<void>(`${this.url}/${id}`);
    }

    update(todo: Todo): Observable<Todo> {
        return this.http.put<Todo>(`${this.url}/${todo.id}`, {
            title: todo.title,
            isCompleted: todo.isCompleted
        });
    }
}
