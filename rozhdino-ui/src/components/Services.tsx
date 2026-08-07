{
  /*از اسمشم معلومه دایی , خدمات سایتت*/
}

import { services } from "../data/services";
import ServiceCard from "./ServiceCard";
import Container from "./ui/Container";
import SectionTitle from "./ui/SectionTitle";

export default function Services() {
  return (
    <section className="py-24 bg-gray-50">
      <Container>
        <SectionTitle
          title="خدمات رشدینو"
          description="خدماتی که برای رشد کسب‌وکار شما ارائه می‌دهیم"
        />
        <div
          className="
          mt-16
          grid
          grid-cols-1
          md:grid-cols-2
          xl:grid-cols-2
          gap-8
          "
        >
          {services.map((service) => (
            <ServiceCard key={service.id} service={service} />
          ))}
        </div>
      </Container>
    </section>
  );
}
