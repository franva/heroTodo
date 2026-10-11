import { Component, inject, signal } from '@angular/core';
import { TodoService } from './services/todo.service';
import { Todo } from './models/todo';

@Component({
  imports: [],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  protected readonly title = signal('Hero Todo App');
  private readonly todoService = inject(TodoService);

  protected readonly todos = signal<Todo[]>([]);
  protected readonly newTitle = signal('');
  protected readonly error = signal('');
  
  ngOnInit() {
    this.todoService.getAll().subscribe({
      next: (todos) => this.todos.set(todos),
      error: () => this.error.set("Database initialization failed. Check back later.")
    });
  }

  protected add(event: Event) {
    event.preventDefault();
    const title = this.newTitle().trim();
    if (!title) return;

    this.todoService.add(title).subscribe({
      next: (todo) => {
        this.todos.update(todos => [...todos, todo]);
        this.newTitle.set('');
        this.error.set('');
      },
      error: () => this.error.set("Failed to add todo. Check back later.")
    });
  }

  protected remove(todo: Todo) {
    this.todoService.remove(todo.id).subscribe({
      next:() => {
        this.todos.update(todos => todos.filter(i => i.id !== todo.id));
      },
      error: () => this.error.set("Failed to remove todo. Check back later.")
    });
  }
}
