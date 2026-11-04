import { ChangeDetectionStrategy, Component, effect, ElementRef, input, output, signal, viewChild } from '@angular/core';
import { Todo } from '../../models/todo';

@Component({
  imports: [],
  selector: 'li[app-todo-item]',
  styleUrl: './todo-item-row.css',
  templateUrl: './todo-item-row.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TodoItemRow {
  readonly todo = input.required<Todo>();
  readonly toggled = output<void>();
  readonly renamed = output<string>();
  readonly deleted = output<void>();

  protected readonly editing = signal(false);
  private readonly editInput = viewChild<ElementRef<HTMLInputElement>>('edit');
  
  constructor() {
    effect(() => this.editInput()?.nativeElement.focus());
  }

  protected save(value: string): void {
    if (!this.editing()) {
      return;
    }

    this.editing.set(false);
    const newTitle = value.trim();
    if(newTitle && newTitle !== this.todo().title) {
      this.renamed.emit(newTitle);
    }
  }
}


