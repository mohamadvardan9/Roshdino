import type { ReactNode } from "react";

interface CardProps {
  children: ReactNode;
  className?: string;
}

export default function Card({ children, className = "" }: CardProps) {
  return (
    <div
      className={`
        bg-white
        rounded-3xl
        border
        p-8
        transition
        hover:shadow-xl
        ${className}
      `}
    >
      {children}
    </div>
  );
}
